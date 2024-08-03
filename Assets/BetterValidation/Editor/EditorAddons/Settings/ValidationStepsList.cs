using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.BehavioredElements;
using Better.Commons.EditorAddons.DropDown;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Validation.EditorAddons.PreBuildValidation;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Validation.EditorAddons.Settings
{
    internal class ValidationStepsList : ListView
    {
        private readonly Type[] _types;
        private readonly SerializedProperty _stepsProperty;
        private readonly GUIContent _popupHeader = new GUIContent("Build Steps");
        private readonly string _validationStepsLabel = "Validation Steps";
        private readonly GUIContent _rootContent = new GUIContent("Root");
        private const string NullName = "null";

        private Dictionary<SerializedProperty, BehavioredElement<Button>> _behavioredElements;

        public ValidationStepsList(SerializedProperty stepsProperty)
        {
            _behavioredElements = new Dictionary<SerializedProperty, BehavioredElement<Button>>();
            _types = typeof(IBuildValidationStep).GetAllInheritedTypesWithoutUnityObject().ToArray();
            _stepsProperty = stepsProperty;

            if (_stepsProperty.GetValue() is not IList value)
            {
                return;
            }

            itemsSource = value;
            makeItem = MakeItem;
            bindItem = BindItem;
            headerTitle = _validationStepsLabel;
            reorderMode = ListViewReorderMode.Animated;
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

            showFoldoutHeader = true;
            showAddRemoveFooter = true;
            showBorder = true;
            reorderable = true;
            style.flexGrow = 1.0f;
        }

        private VisualElement MakeItem()
        {
            return new VisualElement();
        }

        private void BindItem(VisualElement element, int index)
        {
            if (!_stepsProperty.Verify()) return;
            if (!_stepsProperty.isArray) return;
            if (_stepsProperty.arraySize <= index) return;

            var property = _stepsProperty.GetArrayElementAtIndex(index);

            var typeName = GetTypeName(property);
            var behavioredElement = GetOrCreateBehavioredElement(property);

            behavioredElement.SubElement.text = typeName;

            var referenceField = element.Q<SerializeReferenceField>();
            if (referenceField == null)
            {
                referenceField = new SerializeReferenceField(property);

                var userArgs = (property, referenceField);
                referenceField.RegisterCallback<ReferenceTypeChangeEvent, (SerializedProperty, VisualElement)>(OnReferenceTypeChange, userArgs);

                referenceField.PropertyField
                    .RegisterCallback<SerializedPropertyChangeEvent, (SerializedProperty, VisualElement)>(OnSerializedPropertyChanged, userArgs);
                element.Add(referenceField);
                element.OnElementAppear<Label>(behavioredElement.Attach).Until(null).Every(100);
            }
        }

        private void OnSerializedPropertyChanged(SerializedPropertyChangeEvent evt, (SerializedProperty property, VisualElement container) data)
        {
            UpdateElement(data.property, data.container);
        }

        private void UpdateElement(SerializedProperty property, VisualElement label)
        {
            var typeName = GetTypeName(property);
            var element = GetOrCreateBehavioredElement(property);
            element.SubElement.text = typeName;
            element.Attach(label);
        }

        private static string GetTypeName(SerializedProperty property)
        {
            var value = property.managedReferenceValue;
            var typeName = value != null ? value.GetType().Name : NullName;
            return typeName;
        }

        private void OnReferenceTypeChange(ReferenceTypeChangeEvent changeEvent, (SerializedProperty property, VisualElement container) data)
        {
            UpdateElement(data.property, data.container);
        }

        private BehavioredElement<Button> GetOrCreateBehavioredElement(SerializedProperty property)
        {
            if (_behavioredElements.TryGetValue(property, out var element))
            {
                return element;
            }

            element = CreateBehavioredElement(property);
            _behavioredElements.Add(property, element);
            return element;
        }

        private BehavioredElement<Button> CreateBehavioredElement(SerializedProperty property)
        {
            var element = new BehavioredElement<Button>(new SelectElementBehaviour());
            element.RegisterCallback<ClickEvent, (SerializedProperty, BehavioredElement<Button>)>(OnButtonClick, (property, element));
            return element;
        }

        private void OnButtonClick(ClickEvent clickEvent, (SerializedProperty property, BehavioredElement<Button> decorator) data)
        {
            var dataButton = data.decorator;
            ShowDropDown(data.property, dataButton.worldBound);
        }

        private void ShowDropDown(SerializedProperty property, Rect buttonPosition)
        {
            var currentValue = property.managedReferenceValue as Type;
            var copy = buttonPosition;
            copy.y += StyleDefinition.SingleLineHeight.value.value;

            var popup = DropdownWindow.ShowWindow(GUIUtility.GUIToScreenRect(copy), _popupHeader);
            var items = GenerateItemsTree(property, currentValue);

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
                    guiContent.image = IconType.Checkmark.GetIcon();
                }

                var item = new DropdownItem(guiContent, OnSelectItem, new Tuple<SerializedProperty, Type>(serializedProperty, type));
                collection.AddChild(item);
            }

            return collection;
        }

        //TODO: fix it's not updated
        private static void OnSelectItem(object obj)
        {
            if (obj is Tuple<SerializedProperty, Type>(var serializedProperty, var type))
            {
                if (!serializedProperty.Verify()) return;
                serializedProperty.managedReferenceValue = type == null ? null : Activator.CreateInstance(type, true);

                var serializedObject = serializedProperty.serializedObject;
                EditorUtility.SetDirty(serializedObject.targetObject);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}