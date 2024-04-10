using System;
using Better.Commons.EditorAddons.Enums;
using Better.Validation.Runtime.Attributes;


namespace Better.Validation.EditorAddons.Utility
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