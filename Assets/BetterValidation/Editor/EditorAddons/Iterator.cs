using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons
{
    public interface IContextResolver
    {
        public string Resolve(GameObject gameObject);
    }

    public class SceneResolver : IContextResolver
    {
        public static IContextResolver Instance { get; } = new SceneResolver();

        public string Resolve(GameObject gameObject)
        {
            if (gameObject.scene.IsValid())
                return $"{gameObject.scene.path}/{gameObject.FullPath()}";
            return gameObject.FullPath();
        }
    }

    public class AssetResolver : IContextResolver
    {
        public static IContextResolver Instance { get; } = new AssetResolver();

        public string Resolve(GameObject gameObject)
        {
            var hasParent = gameObject.transform.parent != null;
            if (!gameObject.scene.IsValid())
                return AssetDatabase.GetAssetPath(gameObject) + (hasParent ? gameObject.FullPathNoRoot() : string.Empty);
            return gameObject.FullPath();
        }
    }

    public static class Iterator
    {
        private static IContextResolver _context;

        public delegate void OnPropertyIteration(IContextResolver context, SerializedProperty serializedProperty, Component component);

        //TODO: Create context resolver
        public static void SetContext(IContextResolver context)
        {
            _context = context;
        }

        public static void ObjectIteration(GameObject go, OnPropertyIteration onPropertyIteration)
        {
            var components = go.GetComponents<Component>();

            EditorUtility.DisplayProgressBar("Validating components...", "", 0);
            for (var index = 0; index < components.Length; index++)
            {
                var c = components[index];
                if (!c)
                {
                    Debug.LogError("Missing Component in GO: " + _context.Resolve(go), go);
                    continue;
                }

                EditorUtility.DisplayProgressBar("Validating components...", $"Validating {c.name}...", Mathf.Clamp01(index / (float)components.Length));

                var so = new SerializedObject(c);
                var sp = so.GetIterator();

                var copy = sp.Copy();
                if (copy.NextVisible(true))
                {
                    var count = copy.CountInProperty();
                    while (sp.NextVisible(true))
                    {
                        var remainingCopy = sp.Copy();
                        EditorUtility.DisplayProgressBar("Validating property...", $"Validating {sp.propertyPath}...",
                            remainingCopy.CountRemaining() / (float)count);
                        onPropertyIteration?.Invoke(_context, sp, c);
                    }
                }
            }

            EditorUtility.ClearProgressBar();
        }


        public static async Task IterateRoots(IReadOnlyList<GameObject> objs, OnPropertyIteration onPropertyIteration)
        {
            EditorUtility.DisplayProgressBar("Validating objects...", "", 0);
            for (var index = 0; index < objs.Count; index++)
            {
                var go = objs[index];
                EditorUtility.DisplayProgressBar("Validating objects...", $"Validating {go.FullPath()}...", index / (float)objs.Count);
                await Task.Yield();
                ObjectIteration(go, onPropertyIteration);
            }

            EditorUtility.ClearProgressBar();
        }
    }
}