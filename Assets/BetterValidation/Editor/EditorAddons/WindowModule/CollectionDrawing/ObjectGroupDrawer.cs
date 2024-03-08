﻿using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

namespace Better.Validation.EditorAddons.WindowModule.CollectionDrawing
{
    public class ObjectGroupDrawer : GroupDictionaryDrawer<Object, Dictionary<Object, MutableTuple<bool, List<ValidationCommandData>>>>
    {
        public override int Order { get; } = 2;

        public override string GetOptionName()
        {
            return "Object Group";
        }

        protected override Dictionary<Object, MutableTuple<bool, List<ValidationCommandData>>> OnInitialize(List<ValidationCommandData> data)
        {
            var dataDictionary = new Dictionary<Object, MutableTuple<bool, List<ValidationCommandData>>>(GameObjectGroupingComparer.Instance);
            foreach (var commandData in data)
            {
                if (!dataDictionary.TryGetValue(commandData.Target, out var list))
                {
                    list = new MutableTuple<bool, List<ValidationCommandData>>(true, new List<ValidationCommandData>());
                    dataDictionary.Add(commandData.Target, list);
                }

                list.Item2.Add(commandData);
            }

            return dataDictionary;
        }

        protected override string FoldoutName(Object key)
        {
            return key.name;
        }
    }
}