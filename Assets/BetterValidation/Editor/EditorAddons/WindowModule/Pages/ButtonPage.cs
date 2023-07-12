using System;
using System.Collections.Generic;
using System.Linq;
using Better.Extensions.Runtime;
using Better.Validation.EditorAddons.Utilities;
using Better.Validation.EditorAddons.WindowModule.Pages.SubPage;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.WindowModule.Pages
{
    public class ButtonPage : IWindowPage
    {
        private ValidatorCommands _commands;
        private int _groupID;

        private string[] _groupNames;

        private IValidationTab[] _buttons;
        private IValidationTab _currentTab;

        public void Initialize()
        {
            _commands = new ValidatorCommands();
            _buttons = typeof(IValidationTab).GetAllInheritedType().Select(type => (IValidationTab)Activator.CreateInstance(type)).OrderBy(tab => tab.Order)
                .ToArray();
            foreach (var validationButton in _buttons)
            {
                validationButton.Initialize();
            }

            _currentTab = _buttons.FirstOrDefault();
            _groupNames = _buttons.Select(button => button.GetTabName()).ToArray();
        }

        public IWindowPage DrawUpdate()
        {
            List<ValidationCommandData> list = null;
            using (new EditorGUILayout.HorizontalScope())
            {
                _groupID = ToolsGUIUtility.Sidebar(Vector2.zero, _groupID, _groupNames, out var isChanged);
                if (isChanged)
                    _currentTab = _buttons[_groupID];
                list = _currentTab.DrawUpdate();
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