using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.Caching;
using UnityEditor;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class PrefabWrapper : NotNullWrapper
    {
        public override Cache<string> Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var obj = _property.objectReferenceValue;
            if (!PrefabUtility.IsPartOfPrefabAsset(obj))
            {
                var str = DrawersHelper.FormatBoldItalic(_property.displayName);
                if (!PrefabUtility.IsPartOfNonAssetPrefabInstance(obj))
                {
                    CacheField.Set(false, $"Object in \"{str}\" field is not prefab");
                    return CacheField;
                }

                CacheField.Set(false, $"Object in \"{str}\" field is prefab instance in scene");
                return CacheField;
            }

            return GetClearCache();
        }
    }
}