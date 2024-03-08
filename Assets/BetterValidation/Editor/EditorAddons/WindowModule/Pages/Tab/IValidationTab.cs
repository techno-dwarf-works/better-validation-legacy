using System.Collections.Generic;

namespace Better.Validation.EditorAddons.WindowModule
{
    public interface IValidationTab
    {
        public int Order { get; }
        public string GetTabName();
        public void Initialize();
        public List<ValidationCommandData> DrawUpdate();
    }
}