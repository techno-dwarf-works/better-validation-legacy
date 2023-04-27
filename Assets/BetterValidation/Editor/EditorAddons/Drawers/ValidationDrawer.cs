using Better.EditorTools.Drawers.Base;
using Better.EditorTools.Helpers;
using Better.Validation.EditorAddons.Utilities;
using Better.Validation.EditorAddons.ValidationWrappers;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.Drawers
{
    [CustomPropertyDrawer(typeof(ValidationAttribute), true)]
    public class ValidationDrawer : MultiFieldDrawer<ValidationWrapper>
    {
        private float _additional;

        protected override bool PreDraw(ref Rect position, SerializedProperty property, GUIContent label)
        {
            if (!ValidateCachedProperties(property, ValidationUtility.Instance))
            {
                _wrappers[property].Wrapper.SetProperty(property, attribute);
            }

            var validationResult = _wrappers[property].Wrapper.Validate();
            if (!validationResult.Item1)
            {
                _additional = DrawersHelper.GetHelpBoxHeight(position.width, validationResult.Item2, IconType.ErrorMessage) + DrawersHelper.SpaceHeight;
            }

            if (!validationResult.Item1)
            {
                var copy = position;
                copy.y += EditorGUIUtility.singleLineHeight + DrawersHelper.SpaceHeight / 2f;
                DrawersHelper.HelpBox(copy, validationResult.Item2, IconType.ErrorMessage);
            }

            return true;
        }

        protected override Rect PreparePropertyRect(Rect original)
        {
            return original;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label) + _additional;
        }

        protected override void PostDraw(Rect position, SerializedProperty property, GUIContent label)
        {
        }

        protected override WrapperCollection<ValidationWrapper> GenerateCollection()
        {
            return new WrapperCollection<ValidationWrapper>();
        }
    }
}