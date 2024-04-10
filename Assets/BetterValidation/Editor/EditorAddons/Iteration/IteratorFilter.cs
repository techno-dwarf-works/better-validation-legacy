using System.Collections.Generic;
using System.Text;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Extensions;
using Better.Validation.EditorAddons.Utility;
using Better.Validation.EditorAddons.Wrappers;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.Iteration
{
    public static class IteratorFilter
    {
        private class LocalCache : CacheValue<WrapperCollectionValue<PropertyValidationWrapper>>
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

            var fieldType = fieldInfo.FieldInfo.FieldType;
            if (fieldType.IsArrayOrList())
            {
                fieldType = fieldType.GetCollectionElementType();
            }

            var dataList = new List<ValidationCommandData>();
            foreach (var validationAttribute in list)
            {
                ValidateCachedPropertiesUtility.Validate(Wrappers, CacheField, data.Property, fieldType, validationAttribute.GetType(), ValidationAttributeUtility.Instance);

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
            var resolvedPath = commandData.PathResolver.Resolve(target);
            var typeName = target.GetType().Name;
            
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Validation failed with: <b>{0}</b>.", result);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("Path: <i><b>{0}</b></i>.", resolvedPath);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("Component: <i><b>{0}</b></i>, Property: <i><b>{1}</b></i>", typeName, path);

            return stringBuilder.ToString();
        }
    }
}