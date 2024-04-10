using System.Reflection;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Helpers;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Internal.Core.Runtime;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.Wrappers
{
    public class DataValidationWrapper : PropertyValidationWrapper
    {
        public override CacheValue<string> Validate()
        {
            var fieldCache = Property.GetFieldInfoAndStaticTypeFromProperty();
            var att = (DataValidationAttribute)Attribute;

            var propertyContainer = Property.GetPropertyContainer();
            var method = propertyContainer.GetType().GetMethod(att.MethodName, Defines.MethodFlags);
            var methodName = ExtendedGUIUtility.BeautifyFormat(att.MethodName);
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
                    $"Method with name {methodName} has parameter of type {ExtendedGUIUtility.BeautifyFormat(parameterInfo.ParameterType.Name)}. But used on field of type {ExtendedGUIUtility.BeautifyFormat(fieldCache.Type.Name)}");
            }

            if (method.IsStatic)
            {
                return InvokeMethod(method, fieldCache, null);
            }

            return InvokeMethod(method, fieldCache, propertyContainer);
        }

        private CacheValue<string> InvokeMethod(MethodInfo method, CachedFieldInfo fieldCache, object propertyContainer)
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
                    var name = fieldCache.FieldInfo.FieldType.IsArrayOrList() ? Property.GetArrayPath() : propertyContainer.GetType().Name;
                    return GetNotValidCache(
                        $"Validation failed of {ExtendedGUIUtility.BeautifyFormat(fieldCache.FieldInfo.Name)} in {ExtendedGUIUtility.BeautifyFormat(name)}");
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