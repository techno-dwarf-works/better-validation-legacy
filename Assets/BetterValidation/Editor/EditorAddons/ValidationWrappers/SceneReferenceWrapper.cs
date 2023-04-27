using Better.EditorTools.Helpers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class SceneReferenceWrapper : NotNullWrapper
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
                var target = _property.serializedObject.targetObject;

                var obj = _property.objectReferenceValue;

                var targetType = PrefabUtility.GetPrefabAssetType(target);
                var objType = PrefabUtility.GetPrefabAssetType(obj);
                var currentPrefabStage = (bool)PrefabStageUtility.GetCurrentPrefabStage();
                if (currentPrefabStage)
                {
                    if (targetType != PrefabAssetType.NotAPrefab || objType != PrefabAssetType.NotAPrefab)
                    {
                        return ValueTuple(obj, target);
                    }
                }
                else
                {
                    return ValidateNotPrefabContext(obj, target);
                }
            }

            return (true, string.Empty);
        }

        private (bool, string) ValidateNotPrefabContext(Object obj, Object target)
        {
            var isObjectInScene = IsObjectInScene(obj);
            var isTargetInScene = IsObjectInScene(target);

            if (isTargetInScene && !isObjectInScene)
            {
                var str = DrawersHelper.FormatBoldItalic(_property.displayName);
                return (false, $"Object in \"{str}\" field is not scene object");
            }

            if (!isTargetInScene)
            {
                return ValueTuple(obj, target);
            }

            return (true, string.Empty);
        }

        private bool IsObjectInScene(Object obj)
        {
            if (obj is GameObject gameObject)
            {
                return gameObject.scene.IsValid();
            }

            if (obj is Component component)
            {
                return component.gameObject.scene.IsValid();
            }

            return false;
        }

        private (bool, string) ValueTuple(Object obj, Object target)
        {
            var str = DrawersHelper.FormatBoldItalic(_property.displayName);
            var objRoot = GetOutermostPrefabInstanceRoot(obj);
            var targetRoot = GetOutermostPrefabInstanceRoot(target);
            var equals = objRoot == targetRoot;
            if (!equals)
            {
                return (false, $"Object in \"{str}\" field is not part of {target.name} prefab");
            }

            return (true, string.Empty);
        }

        private static Object GetOutermostPrefabInstanceRoot(Object obj)
        {
            var outermostPrefabInstanceRoot = PrefabUtility.GetNearestPrefabInstanceRoot(obj);
            if (outermostPrefabInstanceRoot == null)
            {
                if (obj is GameObject gameObject)
                    outermostPrefabInstanceRoot = gameObject.transform.root ? gameObject.transform.root.gameObject : gameObject;

                if (obj is Component component)
                    outermostPrefabInstanceRoot = component.gameObject;
            }

            return outermostPrefabInstanceRoot;
        }
    }
}