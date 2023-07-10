using System;
using System.Collections.Generic;
using Better.EditorTools.Comparers;
using Better.EditorTools.Drawers.Base;
using Better.EditorTools.Utilities;
using Better.Validation.EditorAddons.ValidationWrappers;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.Utilities
{
    public class ValidationUtility : BaseUtility<ValidationUtility>
    {
        protected override BaseWrappersTypeCollection GenerateCollection()
        {
            return new AttributeWrappersTypeCollection(AssignableFromComparer.Instance)
            {
                { typeof(NotNullAttribute), typeof(NotNullWrapper) },
                { typeof(PrefabReferenceAttribute), typeof(PrefabWrapper) },
                { typeof(SceneReferenceAttribute), typeof(SceneReferenceWrapper) },
                { typeof(FindAttribute), typeof(RequireComponentWrapper) },
                { typeof(DataValidationAttribute), typeof(DataValidationWrapper) },
            };
        }

        public override bool IsSupported(Type type)
        {
            return true;
        }

        protected override HashSet<Type> GenerateAvailable()
        {
            return new HashSet<Type>();
        }
    }
}