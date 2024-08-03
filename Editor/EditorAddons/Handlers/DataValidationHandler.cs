using System.Reflection;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Helpers;
using Better.Commons.Runtime.Extensions;
using Better.Internal.Core.Runtime;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.Handlers
{
    public class ValidationValue<T>
    {
        public bool State { get; set; }

        public T Result { get; set; }

        public void Set(bool state, T result)
        {
            State = state;
            Result = result;
        }
    }
    
    public class DataValidationHandler : PropertyValidationHandler
    {
        public override ValidationValue<string> Validate()
        {
            var fieldCache = Property.GetFieldInfoAndStaticTypeFromProperty();
            var att = (DataValidationAttribute)Attribute;

            var propertyContainer = Property.GetPropertyContainer();
            var method = propertyContainer.GetType().GetMethod(att.MethodName, Defines.MethodFlags);
            var methodName = $"\"{att.MethodName.FormatBoldItalic()}\"";
            if (method == null)
            {
                return GetNotValidValue($"Method with name {methodName} not found");
            }

            var parameters = method.GetParameters();
            if (parameters.Length > 1)
            {
                return GetNotValidValue($"Method with name {methodName} has {parameters.Length}. It's not supported");
            }

            var parameterInfo = parameters[0];
            var parameterType = parameterInfo.ParameterType;
            var fieldCacheType = fieldCache.Type;
            if (parameterType != fieldCacheType)
            {
                return GetNotValidValue(
                    $"Method with name {methodName} has parameter of type \"{parameterType.Name.FormatBoldItalic()}\". But used on field of type \"{fieldCacheType.Name.FormatBoldItalic()}\"");
            }

            if (method.IsStatic)
            {
                return InvokeMethod(method, fieldCache, null);
            }

            return InvokeMethod(method, fieldCache, propertyContainer);
        }

        private ValidationValue<string> InvokeMethod(MethodInfo method, CachedFieldInfo fieldCache, object propertyContainer)
        {
            var value = Property.GetValue();
            var parameters = new object[] { value };
            if (method.ReturnType == typeof(void))
            {
                method.Invoke(propertyContainer, parameters);
            }
            else if (method.ReturnType == typeof(bool))
            {
                var result = (bool)method.Invoke(propertyContainer, parameters);
                if (!result)
                {
                    var fieldInfo = fieldCache.FieldInfo;
                    var name = fieldInfo.FieldType.IsArrayOrList() ? Property.GetArrayPath() : propertyContainer.GetType().Name;
                    return GetNotValidValue(
                        $"Validation failed of \"{fieldInfo.Name.FormatBoldItalic()}\" in \"{name.FormatBoldItalic()}\"");
                }
            }
            else if (method.ReturnType == typeof(string))
            {
                var result = (string)method.Invoke(propertyContainer, parameters);
                if (!string.IsNullOrEmpty(result))
                {
                    return GetNotValidValue(result);
                }
            }

            return GetClearValue();
        }

        public override bool IsSupported()
        {
            return true;
        }
    }
}