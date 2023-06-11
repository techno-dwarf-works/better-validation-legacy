using System;
using Better.EditorTools.Helpers.Caching;
using Better.Validation.EditorAddons.ContextResolver;
using Better.Validation.EditorAddons.Utilities;
using Better.Validation.EditorAddons.ValidationWrappers;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Better.Validation.EditorAddons
{
    public class ValidationCommandData
    {
        private Func<ValidationCommandData, string, string> _compiler;
        public IContextResolver ContextResolver { get; private set; }
        public Object Target { get; private set; }
        public SerializedObject Context { get; }
        public SerializedProperty Property { get; private set; }
        public string Result { get; private set; }
        public ValidationWrapper Wrapper { get; }
        public bool IsValid { get; private set; }

        public ValidationCommandData(IterationData data, ValidationWrapper wrapper)
        {
            ContextResolver = data.ContextResolver;
            Context = data.Context;
            Property = data.Property;
            Target = data.Target;
            Wrapper = wrapper;
        }

        public void SetResultCompiler(Func<ValidationCommandData, string, string> compiler)
        {
            _compiler = compiler;
        }

        public void Revalidate()
        {
            Context.Update();
            var cache = Wrapper.Validate();
            IsValid = cache.IsValid;
            if (!cache.IsValid)
            {
                Result = _compiler.Invoke(this, cache.Value);
            }
        }

        public void SetInitialResult(string value)
        {
            Result = value;
        }
    }
}