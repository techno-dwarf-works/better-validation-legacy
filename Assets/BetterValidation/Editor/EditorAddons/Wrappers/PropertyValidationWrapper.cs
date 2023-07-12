using Better.Validation.Runtime.Attributes;
using UnityEditor;

namespace Better.Validation.EditorAddons.Wrappers
{
    public abstract class PropertyValidationWrapper : ValidationWrapper
    {
        protected internal SerializedProperty Property { get; private set; }
        protected internal ValidationAttribute Attribute { get; private set; }

        public override ValidationType Type => Attribute.ValidationType;

        public virtual void SetProperty(SerializedProperty property, ValidationAttribute attribute)
        {
            Property = property;
            Attribute = attribute;
        }
        
        public override void Deconstruct()
        {
            Property = null;
        }
    }
}