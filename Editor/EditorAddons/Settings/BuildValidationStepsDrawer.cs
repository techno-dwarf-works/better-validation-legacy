using System;
using Better.EditorTools;
using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.DropDown;
using Better.Extensions.Runtime;
using Better.Validation.EditorAddons.PreBuildValidation.Interfaces;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Better.Validation.EditorAddons.Settings
{
    internal class BuildValidationStepsDrawer
    {
        private readonly Type[] _types;
        private readonly ReorderableList _reorderableList;
        private readonly SerializedProperty _stepsProperty;
        private readonly GUIContent _popupHeader = new GUIContent("Build Steps");
        private readonly GUIContent _validationStepsLabel = new GUIContent("Validation Steps");
        private readonly GUIContent _rootContent = new GUIContent("Root");
        private const string NullName = "null";

        public BuildValidationStepsDrawer(SerializedObject settingsObject, SerializedProperty stepsProperty)
        {
            _types = typeof(IBuildValidationStep).GetAllInheritedType();
            _stepsProperty = stepsProperty;

            _reorderableList = new ReorderableList(settingsObject, _stepsProperty, true, true, true, true)
            {
                drawElementCallback = DrawElementCallback,
                drawHeaderCallback = HeaderCallback,
                elementHeightCallback = ElementHeightCallback
            };
        }

        private float ElementHeightCallback(int index)
        {
            var property = _stepsProperty.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(property, true);
        }

        private void HeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, _validationStepsLabel);
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            
            var property = _stepsProperty.GetArrayElementAtIndex(index);
            
            var type = property.GetManagedType();
            DrawButton(rect, property, type);
            var label = new GUIContent(L10n.Tr($"Element {index.ToString()}"));
            EditorGUI.PropertyField(rect, property, label, true);
        }

        private void DrawButton(Rect rect, SerializedProperty serializedProperty, Type currentValue)
        {
            var typeName = currentValue == null ? NullName : currentValue.Name;
            var content = DrawersHelper.GetIconGUIContent(IconType.GrayDropdown);
            content.text = typeName;
            var buttonPosition = GetPopupPosition(rect);
            if (GUI.Button(buttonPosition, content, Styles.Button))
            {
                ShowDropDown(buttonPosition, serializedProperty, currentValue);
            }
        }

        private void ShowDropDown(Rect popupPosition, SerializedProperty serializedProperty, Type currentValue)
        {
            var copy = popupPosition;
            copy.y += EditorGUIUtility.singleLineHeight;

            var popup = DropdownWindow.ShowWindow(GUIUtility.GUIToScreenRect(copy), _popupHeader);
            var items = GenerateItemsTree(serializedProperty, currentValue);

            popup.SetItems(items);
        }

        private DropdownCollection GenerateItemsTree(SerializedProperty serializedProperty, Type currentValue)
        {
            var collection = new DropdownCollection(new DropdownSubTree(_rootContent));
            foreach (var type in _types)
            {
                var typeName = type == null ? NullName : type.Name;
                var guiContent = new GUIContent(typeName);
                if (guiContent.image == null && type == currentValue)
                {
                    guiContent.image = DrawersHelper.GetIcon(IconType.Checkmark);
                }

                var item = new DropdownItem(guiContent, OnSelectItem, new Tuple<SerializedProperty, Type>(serializedProperty, type));
                collection.AddChild(item);
            }

            return collection;
        }

        private static void OnSelectItem(object obj)
        {
            if (obj is Tuple<SerializedProperty, Type>(var serializedProperty, var type))
            {
                if (!serializedProperty.Verify()) return;
                serializedProperty.managedReferenceValue = type == null ? null : Activator.CreateInstance(type);
                
                var serializedObject = serializedProperty.serializedObject;
                EditorUtility.SetDirty(serializedObject.targetObject);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private static Rect GetPopupPosition(Rect currentPosition)
        {
            var popupPosition = new Rect(currentPosition);
            popupPosition.width -= EditorGUIUtility.labelWidth;
            popupPosition.x += EditorGUIUtility.labelWidth;
            popupPosition.height = EditorGUIUtility.singleLineHeight;
            return popupPosition;
        }

        public void DoLayoutList()
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                _reorderableList.DoLayoutList();
                if (scope.changed)
                {
                    var serializedObject = _stepsProperty.serializedObject;
                    EditorUtility.SetDirty(serializedObject.targetObject);
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}