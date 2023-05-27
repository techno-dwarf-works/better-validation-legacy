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
            var cache = ValidateCachedProperties(property, ValidationUtility.Instance);
            var validationWrapper = cache.Value;
            if (!cache.IsValid)
            {
                if (cache.Value == null)
                {
                    return false;
                }

                validationWrapper.Wrapper.SetProperty(property, attribute);
            }

            if (!validationWrapper.Wrapper.IsSupported()) return true;
            var validationResult = validationWrapper.Wrapper.Validate();
            if (!validationResult.IsValid)
            {
                _additional = DrawersHelper.GetHelpBoxHeight(position.width, validationResult.Value, IconType.ErrorMessage) + DrawersHelper.SpaceHeight;
            }

            if (!validationResult.IsValid)
            {
                var copy = position;
                copy.y += EditorGUIUtility.singleLineHeight + DrawersHelper.SpaceHeight / 2f;
                DrawersHelper.HelpBox(copy, validationResult.Value, IconType.ErrorMessage);
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