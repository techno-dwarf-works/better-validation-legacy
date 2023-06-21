using System;
using Better.EditorTools.Helpers;
using Better.Validation.Runtime.Attributes;

namespace Better.Validation.EditorAddons.Utilities
{
    public static class ValidationExtensions
    {
        public static IconType GetIconType(this ValidationType dataType)
        {
            return dataType switch
            {
                ValidationType.Error => IconType.ErrorMessage,
                ValidationType.Warning => IconType.WarningMessage,
                ValidationType.Info => IconType.InfoMessage,
                _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
            };
        }
    }
}