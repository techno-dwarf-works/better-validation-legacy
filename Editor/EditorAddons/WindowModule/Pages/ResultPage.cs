using System;
using System.Collections.Generic;
using System.Linq;
using Better.EditorTools.EditorAddons.Helpers;
using Better.Extensions.Runtime;
using Better.Validation.EditorAddons.Utility;
using Better.Validation.EditorAddons.WindowModule.CollectionDrawing;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.Validation.EditorAddons.WindowModule
{
    public class ResultPage : IWindowPage
    {
        private Vector2 _scrollPosition;
        private GUIStyle _scrollStyle;
        private const string AllClear = "All clear. There is no failed validation!";

        private int _groupID;

        private CollectionDrawer _collectionDrawer;

        private static string[] _groupNames;
        private static CollectionDrawer[] _groups;

        public ResultPage()
        {
            PrefabStage.prefabSaved += OnPrefabSaved;
            EditorSceneManager.sceneSaved += OnSceneSaved;
        }

        public void SetData(List<ValidationCommandData> data)
        {
            _collectionDrawer = _groups.FirstOrDefault()?.Initialize(data);
        }

        public void Initialize()
        {
            _groups = typeof(CollectionDrawer).GetAllInheritedTypesWithoutUnityObject()
                .Select(x => (CollectionDrawer)Activator.CreateInstance(x)).OrderBy(x => x.Order).ToArray();
            _groupNames = _groups.Select(x => x.GetOptionName()).ToArray();
        }

        public IWindowPage DrawUpdate()
        {
            if (_collectionDrawer == null || !_collectionDrawer.IsValid()) return null;
            if (_groups.Length <= 0) return null;
            using (new EditorGUILayout.VerticalScope())
            {
                if (DrawBackControl(out var backPage)) return backPage;

                if (_collectionDrawer.Count <= 0)
                {
                    GUILayout.Label(AllClear);
                }
                else
                {
                    _groupID = ToolsGUIUtility.Toolbar(_groupID, _groupNames, out var isChanged);
                    if (isChanged)
                    {
                        _collectionDrawer = _groups[_groupID].CopyFrom(_collectionDrawer);
                    }

                    DrawCommandList();
                    DrawCommandButtons();
                }
            }

            return null;
        }

        private static bool DrawBackControl(out IWindowPage backPage)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Go Back"))
                {
                    {
                        backPage = new ButtonPage();
                        return true;
                    }
                }

                GUILayout.FlexibleSpace();
            }

            backPage = null;
            return false;
        }

        public void Deconstruct()
        {
            PrefabStage.prefabSaved -= OnPrefabSaved;
            EditorSceneManager.sceneSaved -= OnSceneSaved;
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
                    DrawPrevButton();
                    DrawNextButton();
                }

                DrawClearButton();
            }
        }

        private void DrawNextButton()
        {
            if (!GUILayout.Button("Next")) return;

            var nextObject = _collectionDrawer.GetNext();

            ValidationUtility.OpenReference(nextObject.Target);
        }

        private void DrawPrevButton()
        {
            if (!GUILayout.Button("Previous")) return;

            var nextObject = _collectionDrawer.GetPrevious();

            ValidationUtility.OpenReference(nextObject.Target);
        }

        private void DrawClearButton()
        {
            if (!GUILayout.Button("Clean Resolved")) return;
            _collectionDrawer.ClearResolved();
        }

        private void OnSceneSaved(Scene scene)
        {
            _collectionDrawer.Revalidate();
        }

        private void OnPrefabSaved(GameObject obj)
        {
            _collectionDrawer.Revalidate();
        }
    }
}