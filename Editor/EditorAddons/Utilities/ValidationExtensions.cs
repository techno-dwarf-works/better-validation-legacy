using System;
using System.Collections.Generic;
using System.IO;
using Better.EditorTools.Helpers;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Better.Extensions.Runtime;

#if !UNITY_2021_2_OR_NEWER
using UnityEditor.Experimental.SceneManagement;
#endif


namespace Better.Validation.EditorAddons.Utilities
{
    public static class ValidationExtensions
    {
        public static IconType GetIconType(this ValidationType dataType)
        {
            return dataType switch
            {
                ValidationType.Error => IconType.ErrorMessage,
                ValidationType.Warning => IconType.WarningMessage,
                ValidationType.Info => IconType.InfoMessage,
                _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
            };
        }

        public static void OpenReference(UnityEngine.Object reference)
        {
            if (reference is Component component)
            {
                var transform = component.transform;
                if (PrefabUtility.IsPartOfPrefabAsset(reference))
                {
                    var assetPath = AssetDatabase.GetAssetPath(reference);
                    Transform prefabRootTransform = null;
#if !UNITY_2021_2_OR_NEWER
                    var obj = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                    AssetDatabase.OpenAsset(obj);
                    var stage = PrefabStageUtility.GetCurrentPrefabStage();
                    prefabRootTransform = stage.prefabContentsRoot.transform;
#else
                    var stage = PrefabStageUtility.OpenPrefab(assetPath);
                    prefabRootTransform = stage.prefabContentsRoot.transform;
#endif
                    var indexes = GetParentIndices(transform);

                    transform = GetChildBySiblingIndices(prefabRootTransform, indexes);
                }
                else
                {
                    int countLoaded = SceneManager.sceneCount;
                    Scene[] loadedScenes = new Scene[countLoaded];

                    for (int i = 0; i < countLoaded; i++)
                    {
                        loadedScenes[i] = SceneManager.GetSceneAt(i);
                    }

                    if (loadedScenes.Contains(component.gameObject.scene))
                    {
                        EditorSceneManager.SetActiveScene(component.gameObject.scene);
                    }
                    else
                    {
                        EditorSceneManager.OpenScene(component.gameObject.scene.path);
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

        private static Transform GetChildBySiblingIndices(Transform transform, List<int> siblingIndices)
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