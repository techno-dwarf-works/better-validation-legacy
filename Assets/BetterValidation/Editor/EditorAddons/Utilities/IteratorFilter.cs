using System.Collections.Generic;
using Better.EditorTools;
using Better.EditorTools.Drawers.Base;
using Better.EditorTools.Helpers.Caching;
using Better.Extensions.Runtime;
using Better.Validation.EditorAddons.ValidationWrappers;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.Utilities
{
    public static class IteratorFilter
    {
        private class LocalCache : Cache<WrapperCollectionValue<PropertyValidationWrapper>>
        {
        }

        private static readonly LocalCache CacheField = new LocalCache();
        private static readonly WrapperCollection<PropertyValidationWrapper> Wrappers = new WrapperCollection<PropertyValidationWrapper>();

        private static readonly MissingReferenceWrapper ReferenceWrapper = new MissingReferenceWrapper();

        public static IEnumerable<ValidationCommandData> PropertyIterationWithAttributes(IterationData data)
        {
            var fieldInfo = data.Property.GetFieldInfoAndStaticTypeFromProperty();
            var list = data.Property.GetAttributes<ValidationAttribute>();
            if (fieldInfo == null || list == null) return null;

            var fieldType = fieldInfo.FieldInfo.GetFieldOrElementType();
            var dataList = new List<ValidationCommandData>();
            foreach (var validationAttribute in list)
            {
                Wrappers.ValidateCachedProperties(CacheField, data.Property, fieldType, validationAttribute.GetType(), ValidationUtility.Instance);

                var fieldValue = CacheField.Value;
                if (fieldValue == null)
                {
                    continue;
                }

                var wrapper = fieldValue.Wrapper;
                wrapper.SetProperty(data.Property, validationAttribute);

                var result = wrapper.IsSupported() ? wrapper.Validate() : ValidationWrapper.GetClearCache();

                if (result.IsValid) continue;

                dataList.Add(GenerateData(data, wrapper, result.Value));
            }

            return dataList;
        }

        public static IEnumerable<ValidationCommandData> MissingPropertyIteration(IterationData data)
        {
            ReferenceWrapper.SetProperty(data.Property, null);
            if (!ReferenceWrapper.IsSupported()) return null;
            var result = ReferenceWrapper.Validate();
            if (result.IsValid) return null;
            var validationCommandData = new ValidationCommandData(data, ReferenceWrapper);

            validationCommandData.SetResultCompiler(ObjectCompiler);

            validationCommandData.SetInitialResult(ObjectCompiler(validationCommandData, result.Value));
            return new[]
            {
                validationCommandData
            };
        }

        private static ValidationCommandData GenerateData(IterationData data, PropertyValidationWrapper wrapper, string resultValue)
        {
            var copy = new ValidationCommandData(data, wrapper.Copy());
            copy.SetResultCompiler(ObjectCompiler);
            copy.SetInitialResult(ObjectCompiler(copy, resultValue));
            return copy;
        }

        private static string ObjectCompiler(ValidationCommandData commandData, string result)
        {
            var property = commandData.Property;
            var path = property.IsArrayElement() ? property.GetArrayPath() : property.displayName;
            var target = commandData.Target;
            var str =
                $"Validation failed with: <b>{result}</b>.\nPath: <i><b>{commandData.ContextResolver.Resolve(target)}</b></i>. Component: <i><b>{target.GetType().Name}</b></i>, Property: <i><b>{path}</b></i>";

            return str;
        }
    }
}