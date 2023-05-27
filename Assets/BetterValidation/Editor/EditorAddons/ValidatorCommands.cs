using System.Linq;
using Better.EditorTools;
using Better.EditorTools.Drawers.Base;
using Better.EditorTools.Helpers.Caching;
using Better.Extensions.Runtime;
using Better.Validation.EditorAddons.Utilities;
using Better.Validation.EditorAddons.ValidationWrappers;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.Validation.EditorAddons
{
    public class ValidatorCommands
    {
        protected class LocalCache : Cache<WrapperCollectionValue<ValidationWrapper>>
        {
        }

        private static readonly LocalCache CacheField = new LocalCache();
        private static WrapperCollection<ValidationWrapper> _wrappers = new WrapperCollection<ValidationWrapper>();
        private const string Err = "Missing Ref in: <b>{2}</b>. Component: <i><b>{0}</b></i>, Property: <i><b>{1}</b></i>";

        private const string ValidationErr =
            "Validation failed with: <b>{3}</b>.\nPath: <i><b>{0}</b></i>. Component: <i><b>{1}</b></i>, Property: <i><b>{2}</b></i>";

        private static void OnPropertyIteration(IContextResolver context, SerializedProperty sp, Component component)
        {
            var fieldInfo = sp.GetFieldInfoAndStaticTypeFromProperty();
            var list = sp.GetAttributes<ValidationAttribute>();
            if (fieldInfo == null || list == null) return;

            var fieldType = fieldInfo.FieldInfo.GetArrayOrListElementType();
            foreach (var validationAttribute in list)
            {
                _wrappers.ValidateCachedProperties(CacheField, sp, fieldType, validationAttribute.GetType(), ValidationUtility.Instance);

                if (!CacheField.IsValid)
                {
                    if (CacheField.Value == null)
                    {
                        continue;
                    }

                    CacheField.Value.Wrapper.SetProperty(sp, validationAttribute);
                }

                var result = ValidateCachedProperties(sp, validationAttribute);

                if (result.IsValid) continue;
                LogValidationError(context, sp, component, result);
            }
        }

        private static Cache<string> ValidateCachedProperties(SerializedProperty sp, ValidationAttribute validationAttribute)
        {
            var validationWrapper = CacheField.Value?.Wrapper;
            if (validationWrapper == null || !validationWrapper.IsSupported()) return ValidationWrapper.GetClearCache();

            return validationWrapper.Validate();
        }

        private static void LogValidationError(IContextResolver context, SerializedProperty sp, Component c, Cache<string> result)
        {
            var gameObject = c.gameObject;
            Debug.LogError(string.Format(ValidationErr, context.Resolve(gameObject), c.GetType().Name, sp.GetArrayPath(), result.Value), gameObject);
        }

        public static void FindMissingReferencesInCurrentScene()
        {
            var countLoaded = SceneManager.sceneCount;

            for (var i = 0; i < countLoaded; i++)
            {
                FindMissingReferences(SceneResolver.Instance, SceneManager.GetSceneAt(i).GetRootGameObjects());
            }
        }

        public static async void ValidateAttributesInProject()
        {
            var allAssets = AssetDatabase.GetAllAssetPaths();
            var objs = allAssets.SelectMany(a => AssetDatabase.LoadAssetAtPath<GameObject>(a).GetAllChildren()).ToArray();

            Iterator.SetContext(AssetResolver.Instance);
            await Iterator.IterateRoots(objs, OnPropertyIteration);
        }

        public static async void ValidateAttributesInCurrentScene()
        {
            var scene = SceneManager.GetActiveScene();
            Iterator.SetContext(SceneResolver.Instance);
            await Iterator.IterateRoots(scene.GetRootGameObjects().SelectMany(x => x.GetAllChildren()).ToList(), OnPropertyIteration);
        }

        public static void MissingInAllScenes()
        {
            foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
            {
                var sceneReference = EditorSceneManager.OpenScene(scene.path);
                FindMissingReferences(SceneResolver.Instance, sceneReference.GetRootGameObjects());
            }
        }

        public static void MissingReferencesInAssets()
        {
            var allAssets = AssetDatabase.GetAllAssetPaths();
            var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();

            FindMissingReferences(AssetResolver.Instance, objs);
        }

        private static void FindMissingReferences(IContextResolver context, GameObject[] objects)
        {
            foreach (var go in objects)
            {
                Iterator.SetContext(context);
                Iterator.ObjectIteration(go, PropertyIteration);
            }
        }

        private static void PropertyIteration(IContextResolver contextResolver, SerializedProperty sp, Component c)
        {
            if (sp.propertyType != SerializedPropertyType.ObjectReference) return;
            if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
            {
                var gameObject = c.gameObject;
                Debug.LogError(string.Format(Err, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name), contextResolver.Resolve(gameObject)), gameObject);
            }
        }
    }
}