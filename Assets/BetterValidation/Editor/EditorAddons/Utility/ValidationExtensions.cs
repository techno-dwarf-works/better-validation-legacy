using System;
using Better.Commons.EditorAddons.Enums;
using Better.Validation.Runtime.Attributes;
using UnityEngine.UIElements;


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
        
        public static HelpBoxMessageType GetMessageType(this ValidationType dataType)
        {
            return dataType switch
            {
                ValidationType.Error => HelpBoxMessageType.Error,
                ValidationType.Warning => HelpBoxMessageType.Warning,
                ValidationType.Info => HelpBoxMessageType.Info,
                _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
            };
        }
    }
}