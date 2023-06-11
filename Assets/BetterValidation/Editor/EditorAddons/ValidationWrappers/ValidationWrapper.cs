using Better.EditorTools.Helpers.Caching;
using Better.EditorTools.Utilities;

namespace Better.Validation.EditorAddons.ValidationWrappers
{
    public abstract class ValidationWrapper : UtilityWrapper
    {
        internal static readonly Cache<string> CacheField = new Cache<string>();

        public abstract Cache<string> Validate();

        public abstract bool IsSupported();

        public static Cache<string> GetNotValidCache(string value)
        {
            CacheField.Set(false, value);
            return CacheField;
        }

        public static Cache<string> GetClearCache()
        {
            CacheField.Set(true, string.Empty);
            return CacheField;
        }
    }
}