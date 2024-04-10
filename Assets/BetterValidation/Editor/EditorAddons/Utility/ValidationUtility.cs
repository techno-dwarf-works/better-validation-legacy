using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.Validation.EditorAddons.Utility
{
    public static class ValidationUtility
    {
        public static void OpenReference(UnityEngine.Object reference)
        {
            if (reference.IsNullOrDestroyed())
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(reference));
                return;
            }

            if (reference is Component component)
            {
                var transform = component.transform;
                if (PrefabUtility.IsPartOfPrefabAsset(reference))
                {
                    var assetPath = AssetDatabase.GetAssetPath(reference);
                    var stage = PrefabStageUtility.OpenPrefab(assetPath);
                    var prefabRootTransform = stage.prefabContentsRoot.transform;
                    var indexes = GetParentIndices(transform);

                    transform = GetChildBySiblingIndices(prefabRootTransform, indexes);
                }
                else
                {
                    var countLoaded = SceneManager.sceneCount;
                    var loadedScenes = new Scene[countLoaded];

                    for (var i = 0; i < countLoaded; i++)
                    {
                        loadedScenes[i] = SceneManager.GetSceneAt(i);
                    }

                    var gameObject = component.gameObject;
                    if (loadedScenes.Contains(gameObject.scene))
                    {
                        EditorSceneManager.SetActiveScene(gameObject.scene);
                    }
                    else
                    {
                        EditorSceneManager.OpenScene(gameObject.scene.path);
                    }
                }

                Selection.activeTransform = transform;
                EditorGUIUtility.PingObject(transform);
            }
            else
            {
                Selection.SetActiveObjectWithContext(reference, reference);
                EditorGUIUtility.PingObject(reference);
            }
        }
        
        private static List<int> GetParentIndices(Transform instance)
        {
            if (instance.IsNullOrDestroyed())
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(instance));
                return new List<int>();
            }

            var siblingIndices = new List<int>();

            var parent = instance.parent;
            if (parent == null)
            {
                return siblingIndices;
            }

            // Add the sibling index of the initial transform
            siblingIndices.Add(instance.GetSiblingIndex());
            // Traverse up the hierarchy to add sibling indices of parent transforms
            while (parent != null && parent != instance.root)
            {
                siblingIndices.Add(parent.GetSiblingIndex());
                parent = parent.parent;
            }

            return siblingIndices;
        }

        private static Transform GetChildBySiblingIndices(Transform transform, IReadOnlyList<int> siblingIndices)
        {
            var child = transform;
            for (var i = siblingIndices.Count - 1; i >= 0; i--)
            {
                var siblingIndex = siblingIndices[i];
                if (siblingIndex >= 0 && siblingIndex < child.childCount)
                {
                    child = child.GetChild(siblingIndex);
                }
                else
                {
                    // Sibling index out of range, return null
                    return null;
                }
            }

            return child;
        }

    }
}