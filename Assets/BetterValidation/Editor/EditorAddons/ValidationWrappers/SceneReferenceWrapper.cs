using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.Caching;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class SceneReferenceWrapper : NotNullWrapper
    {
        public override Cache<string> Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            var target = _property.serializedObject.targetObject;

            var obj = _property.objectReferenceValue;

            return ValidateNotPrefabContext(obj, target);
        }

        private Cache<string> ValidateNotPrefabContext(Object obj, Object target)
        {
            var isObjectInScene = IsObjectInScene(obj);
            var isTargetInScene = IsObjectInScene(target);

            if (isTargetInScene && !isObjectInScene)
            {
                var str = DrawersHelper.FormatBoldItalic(_property.displayName);
                CacheField.Set(false, $"Object in \"{str}\" field is not scene object");
                return CacheField;
            }

            if (!isTargetInScene)
            {
                return ValueTuple(obj, target);
            }

            return GetClearCache();
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

        private Cache<string> ValueTuple(Object obj, Object target)
        {
            var str = DrawersHelper.FormatBoldItalic(_property.displayName);
            var objRoot = GetOutermostPrefabInstanceRoot(obj);
            var targetRoot = GetOutermostPrefabInstanceRoot(target);
            var equals = objRoot == targetRoot;
            if (!equals)
            {
                CacheField.Set(false, $"Object in \"{str}\" field is not part of {target.name} prefab");
                return CacheField;
            }

            return  GetClearCache();
        }

        private static Object GetOutermostPrefabInstanceRoot(Object obj)
        {
            var outermostPrefabInstanceRoot = PrefabUtility.GetNearestPrefabInstanceRoot(obj);
            if (outermostPrefabInstanceRoot.IsNullOrDestroyed())
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