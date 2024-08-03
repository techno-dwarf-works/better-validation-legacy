using Better.Validation.Runtime.Attributes;
using UnityEngine;

namespace Better.Validation.EditorAddons.Handlers
{
    public class MissingComponentHandler : ValidationHandler
    {
        private Object _target;

        public MissingComponentHandler(Object target)
        {
            _target = target;
        }

        public override ValidationType Type => ValidationType.Error;

        public override ValidationValue<string> Validate()
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