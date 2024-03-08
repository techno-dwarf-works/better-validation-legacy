using System.Collections.Generic;
using System.Linq;
using Better.EditorTools.EditorAddons.Helpers;
using Better.Extensions.Runtime;
using Better.Validation.EditorAddons.Utility;
using UnityEditor;

namespace Better.Validation.EditorAddons.WindowModule.CollectionDrawing
{
    public abstract class GroupDictionaryDrawer<TKey, TDictionary> : CollectionDrawer
        where TDictionary : class, IDictionary<TKey, MutableTuple<bool, List<ValidationCommandData>>>, new()
    {
        private TDictionary _dataDictionary = null;
        private int _count;

        public override int Count => _count;

        public override ValidationCommandData GetNext()
        {
            return GetAtDirection(1);
        }

        public override ValidationCommandData GetPrevious()
        {
            return GetAtDirection(-1);
        }

        public override CollectionDrawer Initialize(List<ValidationCommandData> data)
        {
            if (data.IsNullOrEmpty())
            {
                _dataDictionary = new TDictionary();
                _count = 0;
            }
            else
            {
                _dataDictionary = OnInitialize(data);
                _count = _dataDictionary.Sum(x => x.Value.Item2.Count);
            }

            return this;
        }

        protected abstract TDictionary OnInitialize(List<ValidationCommandData> data);

        protected abstract string FoldoutName(TKey key);

        public override void DrawCollection()
        {
            foreach (var keyValue in _dataDictionary)
            {
                var value = keyValue.Value;
                if (!value.Item1)
                {
                    value.Item1 = value.Item2.Contains(_currentItem);
                }

                using (var groupScope = new FoldoutHeaderGroupScope(value.Item1, FoldoutName(keyValue.Key)))
                {
                    if (groupScope.IsFolded)
                    {
                        foreach (var validationCommandData in value.Item2)
                        {
                            DrawBox(validationCommandData);
                            EditorGUILayout.Space(DrawersHelper.SpaceHeight);
                        }
                    }

                    value.Item1 = groupScope.IsFolded;
                }
            }
        }

        private ValidationCommandData GetAtDirection(int direction)
        {
            if (_currentItem == null)
            {
                _currentItem = _dataDictionary.First().Value.Item2.First();
            }
            else
            {
                var list = _dataDictionary.Values.SelectMany(x => x.Item2).ToList();

                var index = list.IndexOf(_currentItem);
                index += direction;
                if (index >= list.Count)
                {
                    index = 0;
                }

                _currentItem = list[index];
            }

            return _currentItem;
        }

        public override void ClearResolved()
        {
            foreach (var keyValue in _dataDictionary.Values)
            {
                keyValue.Item2.RemoveAll(x =>
                {
                    x.Revalidate();
                    return x.IsValid;
                });
            }

            _count = _dataDictionary.Sum(x => x.Value.Item2.Count);
        }


        public override void Revalidate()
        {
            foreach (var list in _dataDictionary.Values)
            {
                foreach (var data in list.Item2)
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
            if (!IsValid())
            {
                return new List<ValidationCommandData>();
            }
            
            return _dataDictionary.Values.SelectMany(x => x.Item2).ToList();
        }
    }
}