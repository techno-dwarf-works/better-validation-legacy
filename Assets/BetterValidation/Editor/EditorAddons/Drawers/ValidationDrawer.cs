using Better.EditorTools.Drawers.Base;
using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.Caching;
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
        private Cache<string> _validationResult;

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

                validationWrapper.Wrapper.SetProperty(property, (ValidationAttribute)attribute);
            }

            _validationResult = validationWrapper.Wrapper.Validate().Copy();

            return true;
        }

        protected override Rect PreparePropertyRect(Rect original)
        {
            return original;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        protected override void PostDraw(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!_validationResult.IsValid)
            {
                DrawersHelper.HelpBox(_validationResult.Value, IconType.ErrorMessage);
            }
        }

        protected override WrapperCollection<ValidationWrapper> GenerateCollection()
        {
            return new WrapperCollection<ValidationWrapper>();
        }
    }
}