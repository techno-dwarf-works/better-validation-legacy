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

namespace Better.Validation.EditorAddons
{
    public class ValidationWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private List<ValidationCommandData> _dataList;
        private ValidationCommandData _currentItem = null;
        private GUIStyle _scrollStyle;
        private const string AllClear = "All clear. There is no failed validation!";

        public static void OpenWindow(List<ValidationCommandData> dataList)
        {
            var window = GetWindow<ValidationWindow>(false, nameof(ValidationWindow).PrettyCamelCase());
            window.Show();
            window.Display(dataList);

            PrefabStage.prefabSaved += window.OnPrefabSaved;
            EditorSceneManager.sceneSaved += window.OnSceneSaved;
            window.minSize = new Vector2(200, 400);
        }

        private void OnSceneSaved(Scene scene)
        {
            foreach (var commandData in _dataList)
            {
                commandData.Revalidate();
            }
        }

        private void OnPrefabSaved(GameObject obj)
        {
            foreach (var commandData in _dataList)
            {
                commandData.Revalidate();
            }
        }

        private void OnDestroy()
        {
            PrefabStage.prefabSaved -= OnPrefabSaved;
        }

        private void Display(List<ValidationCommandData> dataList)
        {
            _dataList = dataList;
        }

        private void OnGUI()
        {
            if (_dataList == null) return;
            using (new EditorGUILayout.VerticalScope())
            {
                if (_dataList.Count <= 0)
                {
                    GUILayout.Label(AllClear);
                }
                else
                {
                    DrawCommandList(_dataList);
                    DrawCommandButtons();
                }
            }
        }

        private void DrawCommandList(List<ValidationCommandData> validationCommandDatas)
        {
            using (var scroll = new EditorGUILayout.ScrollViewScope(_scrollPosition))
            {
                _scrollPosition = scroll.scrollPosition;
                _scrollStyle ??= GetScrollStyle();
                using (new EditorGUILayout.VerticalScope(_scrollStyle))
                {
                    foreach (var commandData in validationCommandDatas)
                    {
                        DrawBox(commandData);
                        EditorGUILayout.Space(DrawersHelper.SpaceHeight);
                    }
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
                if (_dataList.Count > 0)
                {
                    DrawNextButton();
                }

                DrawClearButton();
            }
        }

        private void DrawClearButton()
        {
            if (!GUILayout.Button("Clean Resolved")) return;
            _dataList.RemoveAll(x =>
            {
                x.Revalidate();
                return x.IsValid;
            });
        }

        private void DrawNextButton()
        {
            if (!GUILayout.Button("Next")) return;
            if (_currentItem == null)
            {
                _currentItem = _dataList.First();
            }
            else
            {
                var index = _dataList.IndexOf(_currentItem);
                index++;
                if (index >= _dataList.Count)
                {
                    index = 0;
                }

                _currentItem = _dataList[index];
            }

            OpenReference(_currentItem.Target);
        }

        private void DrawBox(ValidationCommandData data)
        {
            var bufferColor = GUI.backgroundColor;

            using (var verticalScore = new EditorGUILayout.VerticalScope())
            {
                if (_currentItem == data)
                {
                    GUI.backgroundColor = Color.yellow;
                }

                GUI.Box(verticalScore.rect, GUIContent.none, EditorStyles.helpBox);

                EditorGUILayout.Space(DrawersHelper.SpaceHeight);

                DrawLabel(data);

                EditorGUILayout.Space(DrawersHelper.SpaceHeight);
                var iconType = IconType.ErrorMessage;
                if (data.IsValid)
                {
                    GUI.backgroundColor = Color.green;
                    iconType = IconType.Checkmark;
                }

                DrawersHelper.HelpBox(data.Result, iconType, false);
                EditorGUILayout.Space(DrawersHelper.SpaceHeight);
            }

            GUI.backgroundColor = bufferColor;
        }

        private void DrawLabel(ValidationCommandData data)
        {
            using (var horizontalScore = new EditorGUILayout.HorizontalScope())
            {
                var reference = data.Target;

                var icon = EditorGUIUtility.GetIconForObject(reference);
                var csIcon = EditorGUIUtility.IconContent("cs Script Icon");
                csIcon.text = reference.GetType().Name;
                if (icon)
                {
                    csIcon.image = icon;
                }
                
                EditorGUILayout.LabelField(csIcon);
                EditorGUILayout.Space(DrawersHelper.SpaceHeight);

                if (GUILayout.Button("Show"))
                {
                    OpenReference(reference);
                    _currentItem = data;
                }
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
            for (int i = siblingIndices.Count - 1; i >= 0; i--)
            {
                int siblingIndex = siblingIndices[i];
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

        private void OpenReference(Object reference)
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
    }
}