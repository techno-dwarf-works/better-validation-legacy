using Better.Tools.Runtime.Settings;
using UnityEngine;

namespace Better.Validation.EditorAddons.Settings
{
    public class BetterValidationSettings : BetterSettings
    {
        [SerializeField] private bool ignoreWarnings;

        public bool IgnoreWarnings
        {
            get => ignoreWarnings;
        }
    }
}