using System;
using Better.EditorTools.Helpers.Caching;
using Better.EditorTools.Utilities;
using Better.Validation.Runtime.Attributes;
using UnityEditor;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public abstract class ValidationWrapper : UtilityWrapper
    {
        internal static readonly Cache<string> CacheField = new Cache<string>();
        protected SerializedProperty _property;
        protected ValidationAttribute _attribute;

        public abstract Cache<string> Validate();

        public virtual void SetProperty(SerializedProperty property, ValidationAttribute attribute)
        {
            _property = property;
            _attribute = attribute;
        }
        
        public static Cache<string> GetNotValidCache(string value)
        {
            CacheField.Set(false, value);
            return CacheField;
        }

        public static Cache<string> GetClearCache()
        {
            CacheField.Set(true, string.Empty);
            return CacheField;
        }

        public override void Deconstruct()
        {
            _property = null;
        }
    }
}