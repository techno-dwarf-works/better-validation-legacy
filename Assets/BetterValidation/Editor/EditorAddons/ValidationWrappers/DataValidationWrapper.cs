using System.Reflection;
using Better.EditorTools;
using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.Caching;
using Better.EditorTools.Runtime;
using Better.Extensions.Runtime;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public class DataValidationWrapper : ValidationWrapper
    {
        public override Cache<string> Validate()
        {
            var fieldCache = _property.GetFieldInfoAndStaticTypeFromProperty();
            var att = (DataValidationAttribute)_attribute;

            var propertyContainer = _property.GetPropertyContainer();
            var method = propertyContainer.GetType().GetMethod(att.MethodName, BetterEditorDefines.MethodFlags);
            var methodName = DrawersHelper.BeautifyFormat(att.MethodName);
            if (method == null)
            {
                return GetNotValidCache($"Method with name {methodName} not found");
            }

            var parameters = method.GetParameters();
            if (parameters.Length > 1)
            {
                return GetNotValidCache($"Method with name {methodName} has {parameters.Length}. It's not supported");
            }

            var parameterInfo = parameters[0];
            if (parameterInfo.ParameterType != fieldCache.Type)
            {
                return GetNotValidCache(
                    $"Method with name {methodName} has parameter of type {DrawersHelper.BeautifyFormat(parameterInfo.ParameterType.Name)}. But used on field of type {DrawersHelper.BeautifyFormat(fieldCache.Type.Name)}");
            }

            if (method.IsStatic)
            {
                return InvokeMethod(method, fieldCache, null);
            }

            return InvokeMethod(method, fieldCache, propertyContainer);
        }

        private Cache<string> InvokeMethod(MethodInfo method, FieldInfoCache fieldCache, object propertyContainer)
        {
            var value = _property.GetValue();
            if (method.ReturnType == typeof(void))
            {
                method.Invoke(propertyContainer, new object[] { value });
            }
            else if (method.ReturnType == typeof(bool))
            {
                var result = (bool)method.Invoke(propertyContainer, new object[] { value });
                if (!result)
                {
                    var name = fieldCache.FieldInfo.IsArrayOrList() ? _property.GetArrayPath() : propertyContainer.GetType().Name;
                    return GetNotValidCache(
                        $"Validation failed of {DrawersHelper.BeautifyFormat(fieldCache.FieldInfo.Name)} in {DrawersHelper.BeautifyFormat(name)}");
                }
            }
            else if (method.ReturnType == typeof(string))
            {
                var result = (string)method.Invoke(propertyContainer, new object[] { value });
                if (!string.IsNullOrEmpty(result))
                {
                    return GetNotValidCache(result);
                }
            }

            return GetClearCache();
        }
    }
}