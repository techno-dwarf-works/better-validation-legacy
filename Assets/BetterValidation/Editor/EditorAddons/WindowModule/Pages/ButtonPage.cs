using System;
using System.Collections.Generic;
using System.Linq;
using Better.EditorTools.EditorAddons.Helpers;
using Better.Extensions.Runtime;
using Better.Validation.EditorAddons.Utility;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.WindowModule
{
    public class ButtonPage : IWindowPage
    {
        private int _groupID;

        private string[] _groupNames;

        private IValidationTab[] _buttons;
        private IValidationTab _currentTab;
        private Vector2 _scrollPosition;

        public void Initialize()
        {
            _buttons = typeof(IValidationTab).GetAllInheritedTypesWithoutUnityObject()
                .Select(type => (IValidationTab)Activator.CreateInstance(type)).OrderBy(tab => tab.Order).ToArray();
            
            foreach (var validationButton in _buttons)
            {
                validationButton.Initialize();
            }

            _currentTab = _buttons.FirstOrDefault();
            _groupNames = _buttons.Select(button => button.GetTabName()).ToArray();
        }

        public IWindowPage DrawUpdate()
        {
            if (_buttons.Length <= 0) return null;
            List<ValidationCommandData> list = null;
            using (new EditorGUILayout.HorizontalScope())
            {
                _groupID = ToolsGUIUtility.Sidebar(ref _scrollPosition, _groupID, _groupNames, out var isChanged);
                if (isChanged)
                {
                    _currentTab = _buttons[_groupID];
                }
                
                using (new EditorGUILayout.HorizontalScope(Styles.DefaultContentMargins))
                {
                    list = _currentTab.DrawUpdate();
                }
            }

            if (list != null)
            {
                var page = new ResultPage();
                page.Initialize();
                page.SetData(list);
                return page;
            }

            return null;
        }

        public void Deconstruct()
        {
        }
    }
}