﻿using Better.Commons.Runtime.Extensions;
using UnityEditor;

namespace Better.Validation.EditorAddons.Handlers
{
    public class PrefabHandler : NotNullHandler
    {
        public override ValidationValue<string> Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.State)
            {
                return baseResult;
            }

            var obj = Property.objectReferenceValue;
            if (!PrefabUtility.IsPartOfPrefabAsset(obj))
            {
                var str = $"\"{Property.displayName.FormatBoldItalic()}\"";
                if (!PrefabUtility.IsPartOfNonAssetPrefabInstance(obj))
                {
                    return GetNotValidValue($"Object in {str} field is not prefab");
                }

                return GetNotValidValue($"Object in {str} field is prefab instance in scene");
            }

            return GetClearValue();
        }
    }
}