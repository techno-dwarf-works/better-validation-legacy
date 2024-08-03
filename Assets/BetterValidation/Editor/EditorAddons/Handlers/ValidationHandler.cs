using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.Handlers
{
    public abstract class ValidationHandler : SerializedPropertyHandler
    {
        internal static readonly ValidationValue<string> ValidationValue = new ValidationValue<string>();

        public abstract ValidationType Type { get; }

        public abstract ValidationValue<string> Validate();

        public abstract bool IsSupported();

        public static ValidationValue<string> GetNotValidValue(string value)
        {
            ValidationValue.Set(false, value);
            return ValidationValue;
        }

        public static ValidationValue<string> GetClearValue()
        {
            ValidationValue.Set(true, string.Empty);
            return ValidationValue;
        }
    }
}