using System;
using System.Collections.Generic;
using System.Linq;
using Better.EditorTools.Helpers;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Better.Validation.EditorAddons.WindowModule
{
    public class ValidationWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private GUIStyle _scrollStyle;
        private const string AllClear = "All clear. There is no failed validation!";
        private const string WindowIcon = "d_console.erroricon.inactive.sml";

        private int _groupID;

        private CollectionDrawer _collectionDrawer;

        private static string[] _groupNames;
        private static CollectionDrawer[] _groups;

        public static void OpenWindow(List<ValidationCommandData> dataList)
        {
            _groups = typeof(CollectionDrawer).GetAllInheritedType().Select(x => (CollectionDrawer)Activator.CreateInstance(x)).OrderBy(x => x.Order).ToArray();
            _groupNames = _groups.Select(x => x.GetOptionName()).ToArray();
            var window = GetWindow<ValidationWindow>(false, nameof(Validation).PrettyCamelCase());
            var content = EditorGUIUtility.IconContent(WindowIcon);
            content.text = nameof(Validation).PrettyCamelCase();
            window.titleContent = content;
            window.Show();
            window.Display(dataList);

            PrefabStage.prefabSaved += window.OnPrefabSaved;
            EditorSceneManager.sceneSaved += window.OnSceneSaved;
            window.minSize = new Vector2(200, 400);
        }

        private void OnSceneSaved(Scene scene)
        {
            _collectionDrawer.Revalidate();
        }

        private void OnPrefabSaved(GameObject obj)
        {
            _collectionDrawer.Revalidate();
        }

        private void OnDestroy()
        {
            PrefabStage.prefabSaved -= OnPrefabSaved;
        }

        private void Display(List<ValidationCommandData> dataList)
        {
            _collectionDrawer = _groups.FirstOrDefault()?.Initialize(dataList);
        }

        private void OnGUI()
        {
            if (_collectionDrawer == null || !_collectionDrawer.IsValid()) return;
            using (new EditorGUILayout.VerticalScope())
            {
                if (_collectionDrawer.Count <= 0)
                {
                    GUILayout.Label(AllClear);
                }
                else
                {
                    _groupID = DrawGrouping(_groupID, out var isChanged);
                    if (isChanged)
                    {
                        _collectionDrawer = _groups[_groupID].CopyFrom(_collectionDrawer);
                    }
                    DrawCommandList();
                    DrawCommandButtons();
                }
            }
        }

        private int DrawGrouping(int groupID, out bool changed)
        {
            var id = GUILayout.Toolbar(groupID, _groupNames);
            changed = id != _groupID;
            return id;
        }

        private void DrawCommandList()
        {
            using (var scroll = new EditorGUILayout.ScrollViewScope(_scrollPosition))
            {
                _scrollPosition = scroll.scrollPosition;
                _scrollStyle ??= GetScrollStyle();
                using (new EditorGUILayout.VerticalScope(_scrollStyle))
                {
                    _collectionDrawer.DrawCollection();
                }
            }
        }

        private static GUIStyle GetScrollStyle()
        {
            var style = new GUIStyle();
            var space = (int)DrawersHelper.SpaceHeight;
            style.margin = new RectOffset(space, space, space, space);
            return style;
        }

        private void DrawCommandButtons()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (_collectionDrawer.Count > 0)
                {
                    DrawNextButton();
                }

                DrawClearButton();
            }
        }

        public void DrawNextButton()
        {
            if (!GUILayout.Button("Next")) return;

            var nextObject = _collectionDrawer.GetNext();

            OpenReference(nextObject.Target);
        }

        private void DrawClearButton()
        {
            if (!GUILayout.Button("Clean Resolved")) return;
            _collectionDrawer.ClearResolved();
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


        public static void OpenReference(Object reference)
        {
            if (reference is Component component)
            {
                var transform = component.transform;
                if (PrefabUtility.IsPartOfPrefabAsset(reference))
                {
                    var stage = PrefabStageUtility.OpenPrefab(AssetDatabase.GetAssetPath(reference));
                    var indexes = GetParentIndices(transform);
                    transform = GetChildBySiblingIndices(stage.prefabContentsRoot.transform, indexes);
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