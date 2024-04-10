using System;
using System.Collections.Generic;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.Drawers.WrappersTypeCollection;
using Better.Commons.Runtime.Comparers;
using Better.Validation.EditorAddons.Wrappers;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.Utility
{
    public class ValidationAttributeUtility : BaseUtility<ValidationAttributeUtility>
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