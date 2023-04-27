using Better.EditorTools.Helpers;
using UnityEditor;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class PrefabWrapper : NotNullWrapper
    {
        public override (bool, string) Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.Item1)
            {
                return baseResult;
            }

            if (_property.propertyType == SerializedPropertyType.ObjectReference)
            {
                var obj = _property.objectReferenceValue;
                if (!PrefabUtility.IsPartOfPrefabAsset(obj))
                {
                    var str = DrawersHelper.FormatBoldItalic(_property.displayName);
                    if (!PrefabUtility.IsPartOfNonAssetPrefabInstance(obj))
                    {
                        return (false, $"Object in \"{str}\" field is not prefab");
                    }

                    return (false, $"Object in \"{str}\" field is prefab instance in scene");
                }
            }

            return (true, string.Empty);
        }
    }
}