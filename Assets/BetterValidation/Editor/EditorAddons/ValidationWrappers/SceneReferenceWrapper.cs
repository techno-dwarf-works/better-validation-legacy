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

            var target = Property.serializedObject.targetObject;

            var obj = Property.objectReferenceValue;

            return ValidateNotPrefabContext(obj, target);
        }

        private Cache<string> ValidateNotPrefabContext(Object obj, Object target)
        {
            var isObjectInScene = IsObjectInScene(obj);
            var isTargetInScene = IsObjectInScene(target);

            if (isTargetInScene && !isObjectInScene)
            {
                var str = DrawersHelper.BeautifyFormat(Property.displayName);
                return GetNotValidCache($"Object in {str} field is not scene object");
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
            var objRoot = GetOutermostPrefabInstanceRoot(obj);
            var targetRoot = GetOutermostPrefabInstanceRoot(target);
            var equals = objRoot == targetRoot;
            if (!equals)
            {
                return GetNotValidCache(
                    $"Object in {DrawersHelper.BeautifyFormat(Property.displayName)} field is not part of {DrawersHelper.BeautifyFormat(target.name)} prefab");
            }

            return GetClearCache();
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