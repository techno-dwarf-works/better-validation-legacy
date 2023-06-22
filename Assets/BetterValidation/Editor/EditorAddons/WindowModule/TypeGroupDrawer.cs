using System.Collections.Generic;
using System.Linq;
using Better.EditorTools.Helpers;
using Better.Validation.Runtime.Attributes;
using UnityEditor;

namespace Better.Validation.EditorAddons.WindowModule
{
    public class TypeGroupDrawer : CollectionDrawer
    {
        private SortedDictionary<ValidationType, BetterTuple<bool, List<ValidationCommandData>>> _dataDictionary;
        private int _count;

        public override int Order { get; } = 1;

        public override string GetOptionName()
        {
            return "Type Group";
        }

        public override CollectionDrawer Initialize(List<ValidationCommandData> data)
        {
            _dataDictionary = new SortedDictionary<ValidationType, BetterTuple<bool, List<ValidationCommandData>>>(Comparer<ValidationType>.Create((x, y) => y.CompareTo(x)));
            foreach (var commandData in data)
            {
                var iconType = commandData.Type;
                if (!_dataDictionary.TryGetValue(iconType, out var list))
                {
                    list = new BetterTuple<bool, List<ValidationCommandData>>(iconType == ValidationType.Error || commandData == _currentItem, new List<ValidationCommandData>());
                    _dataDictionary.Add(iconType, list);
                }

                list.Item2.Add(commandData);
            }
            
            _count = _dataDictionary.Sum(x => x.Value.Item2.Count);
            return this;
        }

        public override int Count => _count;


        public override void DrawCollection()
        {
            foreach (var keyValue in _dataDictionary)
            {
                using (var groupScope = new FoldoutHeaderGroupScope(keyValue.Value.Item1, keyValue.Key.ToString()))
                {
                    if (groupScope.IsFolded)
                    {
                        foreach (var validationCommandData in keyValue.Value.Item2)
                        {
                            DrawBox(validationCommandData);
                            EditorGUILayout.Space(DrawersHelper.SpaceHeight);
                        }
                    }

                    keyValue.Value.Item1 = groupScope.IsFolded;
                }
            }
        }

        public override void ClearResolved()
        {
            foreach (var keyValue in _dataDictionary)
            {
                keyValue.Value.Item2.RemoveAll(x =>
                {
                    x.Revalidate();
                    return x.IsValid;
                });
            }

            _count = _dataDictionary.Sum(x => x.Value.Item2.Count);
        }

        public override ValidationCommandData GetNext()
        {
            if (_currentItem == null)
            {
                _currentItem = _dataDictionary.First().Value.Item2.First();
            }
            else
            {
                var list = _dataDictionary.Values.SelectMany(x => x.Item2).ToList();

                var index = list.IndexOf(_currentItem);
                index++;
                if (index >= list.Count)
                {
                    index = 0;
                }

                _currentItem = list[index];
            }

            return _currentItem;
        }

        public override void Revalidate()
        {
            foreach (var (_, value) in _dataDictionary)
            {
                foreach (var data in value.Item2)
                {
                    data.Revalidate();
                }
            }
        }

        public override bool IsValid()
        {
            return _dataDictionary != null;
        }

        protected override List<ValidationCommandData> GetRemaining()
        {
            return _dataDictionary.Values.SelectMany(x => x.Item2).ToList();
        }
    }
}