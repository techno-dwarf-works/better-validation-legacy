using System.Collections.Generic;
using System.Linq;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.WindowModule.CollectionDrawing
{
    public class TypeGroupDrawer : GroupDictionaryDrawer<ValidationType, SortedDictionary<ValidationType, BetterTuple<bool, List<ValidationCommandData>>>>
    {
        public override int Order { get; } = 1;

        public override string GetOptionName()
        {
            return "Type Group";
        }

        public override CollectionDrawer Initialize(List<ValidationCommandData> data)
        {
            _dataDictionary =
                new SortedDictionary<ValidationType, BetterTuple<bool, List<ValidationCommandData>>>(Comparer<ValidationType>.Create((x, y) => y.CompareTo(x)));
            foreach (var commandData in data)
            {
                var iconType = commandData.Type;
                if (!_dataDictionary.TryGetValue(iconType, out var list))
                {
                    list = new BetterTuple<bool, List<ValidationCommandData>>(iconType == ValidationType.Error || commandData == _currentItem,
                        new List<ValidationCommandData>());
                    _dataDictionary.Add(iconType, list);
                }

                list.Item2.Add(commandData);
            }

            _count = _dataDictionary.Sum(x => x.Value.Item2.Count);
            return this;
        }

        public override int Count => _count;


        protected override string FoldoutName(ValidationType key)
        {
            return key.ToString();
        }
    }
}