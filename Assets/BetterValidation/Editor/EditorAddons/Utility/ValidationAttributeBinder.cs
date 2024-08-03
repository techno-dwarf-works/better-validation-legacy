using System;
using System.Collections.Generic;
using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Commons.EditorAddons.Drawers.HandlersTypeCollection;
using Better.Commons.Runtime.Comparers;
using Better.Validation.EditorAddons.Handlers;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.Utility
{
    [Binder(typeof(ValidationHandler))]
    public class ValidationAttributeBinder : TypeHandlerBinder<ValidationHandler>
    {
        protected override BaseHandlersTypeCollection GenerateCollection()
        {
            return new AttributeHandlersTypeCollection(AssignableFromComparer.Instance)
            {
                { typeof(NotNullAttribute), typeof(NotNullHandler) },
                { typeof(PrefabReferenceAttribute), typeof(PrefabHandler) },
                { typeof(SceneReferenceAttribute), typeof(SceneReferenceHandler) },
                { typeof(FindAttribute), typeof(FindComponentHandler) },
                { typeof(DataValidationAttribute), typeof(DataValidationHandler) },
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