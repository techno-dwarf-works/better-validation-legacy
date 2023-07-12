using System.Collections.Generic;

namespace Better.Validation.EditorAddons.WindowModule.Pages.SubPage
{
    public interface IValidationTab
    {
        public int Order { get; }
        public string GetTabName();
        public void Initialize();
        public List<ValidationCommandData> DrawUpdate();
    }
}