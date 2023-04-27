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
        protected override WrappersTypeCollection GenerateCollection()
        {
            return new WrappersTypeCollection(AssignableFromComparer.Instance)
            {
                {
                    typeof(NotNullAttribute), new Dictionary<Type, Type>(AssignableFromComparer.Instance)
                    {
                        { typeof(UnityEngine.Object), typeof(NotNullWrapper) }
                    }
                },
                {
                    typeof(PrefabFieldAttribute), new Dictionary<Type, Type>(AssignableFromComparer.Instance)
                    {
                        { typeof(UnityEngine.Object), typeof(PrefabWrapper) }
                    }
                },
                {
                    typeof(SceneReferenceAttribute), new Dictionary<Type, Type>(AssignableFromComparer.Instance)
                    {
                        { typeof(UnityEngine.Object), typeof(SceneReferenceWrapper) }
                    }
                },
                {
                    typeof(BaseFindAttribute), new Dictionary<Type, Type>(AssignableFromComparer.Instance)
                    {
                        { typeof(UnityEngine.Object), typeof(RequireComponentWrapper) }
                    }
                }
            };
        }

        protected override HashSet<Type> GenerateAvailable()
        {
            return new HashSet<Type>(AssignableFromComparer.Instance)
            {
                typeof(UnityEngine.Object),
            };
        }
    }
}