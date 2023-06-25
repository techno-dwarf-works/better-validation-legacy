using System.Collections.Generic;
using Better.Extensions.Runtime;
using Better.Validation.EditorAddons.WindowModule.Pages;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.WindowModule
{
    public class ValidationWindow : EditorWindow
    {
        private const string WindowIcon = "d_console.erroricon.inactive.sml";

        private IWindowPage _currentPage;

        public static void OpenWindow(List<ValidationCommandData> dataList)
        {
            var window = GetWindow<ValidationWindow>(false, nameof(Validation).PrettyCamelCase());
            var content = EditorGUIUtility.IconContent(WindowIcon);
            content.text = nameof(Validation).PrettyCamelCase();
            window.titleContent = content;
            window.Show();
            window.Initialize();
            window.Display(dataList);

            window.minSize = new Vector2(200, 400);
        }

        private void Display(List<ValidationCommandData> dataList)
        {
            var page = CreateInitialized<ResultPage>();
            page.SetData(dataList);
            _currentPage = page;
        }

        private T CreateInitialized<T>() where T : class, IWindowPage, new()
        {
            var page = new T();
            page.Initialize();
            return page;
        }

        private void Initialize()
        {
            _currentPage = CreateInitialized<ButtonPage>();
        }

        public static void OpenWindow()
        {
            var window = GetWindow<ValidationWindow>(false, nameof(Validation).PrettyCamelCase());
            var content = EditorGUIUtility.IconContent(WindowIcon);
            content.text = nameof(Validation).PrettyCamelCase();
            window.titleContent = content;
            window.Show();
            window.Initialize();
            window.minSize = new Vector2(200, 400);
        }

        private void OnDestroy()
        {
            _currentPage?.Deconstruct();
        }

        private void OnGUI()
        {
            if (_currentPage == null) return;
            var page = _currentPage.DrawUpdate();
            if (page != null)
            {
                _currentPage.Deconstruct();
                _currentPage = page;
                _currentPage.Initialize();
            }
        }
    }
}