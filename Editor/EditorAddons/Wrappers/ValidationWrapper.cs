using Better.EditorTools.EditorAddons.Helpers.Caching;
using Better.EditorTools.EditorAddons.Utilities;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.Wrappers
{
    public abstract class ValidationWrapper : UtilityWrapper
    {
        internal static readonly CacheValue<string> CacheField = new CacheValue<string>();

        public abstract ValidationType Type { get; }

        public abstract CacheValue<string> Validate();

        public abstract bool IsSupported();

        public static CacheValue<string> GetNotValidCache(string value)
        {
            CacheField.Set(false, value);
            return CacheField;
        }

        public static CacheValue<string> GetClearCache()
        {
            CacheField.Set(true, string.Empty);
            return CacheField;
        }
    }
}