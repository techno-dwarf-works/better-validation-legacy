using System.Reflection;
using Better.EditorTools;
using Better.EditorTools.Helpers;
using Better.EditorTools.Helpers.Caching;
using Better.Extensions.Runtime;
using Better.Tools.Runtime;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.Wrappers
{
    public class DataValidationWrapper : PropertyValidationWrapper
    {
        public override Cache<string> Validate()
        {
            var fieldCache = Property.GetFieldInfoAndStaticTypeFromProperty();
            var att = (DataValidationAttribute)Attribute;

            var propertyContainer = Property.GetPropertyContainer();
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
                    var name = fieldCache.FieldInfo.IsArrayOrList() ? Property.GetArrayPath() : propertyContainer.GetType().Name;
                    return GetNotValidCache(
                        $"Validation failed of {DrawersHelper.BeautifyFormat(fieldCache.FieldInfo.Name)} in {DrawersHelper.BeautifyFormat(name)}");
                }
            }
            else if (method.ReturnType == typeof(string))
            {
                var result = (string)method.Invoke(propertyContainer, parameters);
                if (!string.IsNullOrEmpty(result))
                {
                    return GetNotValidCache(result);
                }
            }

            return GetClearCache();
        }

        public override bool IsSupported()
        {
            return true;
        }
    }
}