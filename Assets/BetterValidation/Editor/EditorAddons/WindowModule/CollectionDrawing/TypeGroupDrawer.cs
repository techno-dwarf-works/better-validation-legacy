using System.Collections.Generic;
using System.Linq;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.WindowModule.CollectionDrawing
{
    public class TypeGroupDrawer : GroupDictionaryDrawer<ValidationType, SortedDictionary<ValidationType, MutableTuple<bool, List<ValidationCommandData>>>>
    {
        public override int Order { get; } = 1;

        public override string GetOptionName()
        {
            return "Type Group";
        }

        protected override SortedDictionary<ValidationType, MutableTuple<bool, List<ValidationCommandData>>> OnInitialize(List<ValidationCommandData> data)
        {
            var dataDictionary =
                new SortedDictionary<ValidationType, MutableTuple<bool, List<ValidationCommandData>>>(Comparer<ValidationType>.Create((x, y) => y.CompareTo(x)));
            foreach (var commandData in data)
            {
                var iconType = commandData.Type;
                if (!dataDictionary.TryGetValue(iconType, out var list))
                {
                    list = new MutableTuple<bool, List<ValidationCommandData>>(iconType == ValidationType.Error || commandData == _currentItem, new List<ValidationCommandData>());
                    dataDictionary.Add(iconType, list);
                }

                list.Item2.Add(commandData);
            }

            return dataDictionary;
        }


        protected override string FoldoutName(ValidationType key)
        {
            return key.ToString();
        }
    }
}