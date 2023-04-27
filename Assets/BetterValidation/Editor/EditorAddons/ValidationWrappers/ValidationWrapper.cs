using System;
using Better.EditorTools.Utilities;
using UnityEditor;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public abstract class ValidationWrapper : UtilityWrapper
    {
        protected SerializedProperty _property;
        protected Attribute _attribute;
        public abstract (bool, string) Validate();

        public virtual void SetProperty(SerializedProperty property, Attribute attribute)
        {
            _property = property;
            _attribute = attribute;
        }

        public override void Deconstruct()
        {
            _property = null;
        }
    }
}