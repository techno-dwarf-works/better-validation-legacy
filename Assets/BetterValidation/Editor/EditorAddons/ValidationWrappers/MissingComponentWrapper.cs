using Better.EditorTools.Helpers.Caching;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class MissingComponentWrapper : ValidationWrapper
    {
        private Object _target;

        public MissingComponentWrapper(Object target)
        {
            _target = target;
        }

        public override Cache<string> Validate()
        {
            if (!_target) return GetClearCache();
            if (_target is GameObject gameObject)
            {
                var components = gameObject.GetComponents<Component>();
                for (var index = components.Length - 1; index >= 0; index--)
                {
                    var obj = components[index];
                    if (!obj) return GetNotValidCache($"Missing Component on GameObject: {_target.name}");
                }
            }

            return GetClearCache();
        }

        public override bool IsSupported()
        {
            return true;
        }

        public override void Deconstruct()
        {
        }
    }
}