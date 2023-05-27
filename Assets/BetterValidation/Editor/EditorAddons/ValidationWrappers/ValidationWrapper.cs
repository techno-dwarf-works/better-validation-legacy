using System;
using Better.EditorTools.Helpers.Caching;
using Better.EditorTools.Utilities;
using UnityEditor;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public abstract class ValidationWrapper : UtilityWrapper
    {
        protected static readonly Cache<string> CacheField = new Cache<string>();
        protected SerializedProperty _property;
        protected Attribute _attribute;

        public abstract bool IsSupported();

        public abstract Cache<string> Validate();

        public virtual void SetProperty(SerializedProperty property, Attribute attribute)
        {
            _property = property;
            _attribute = attribute;
        }

        protected static Cache<string> GetClearCache()
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