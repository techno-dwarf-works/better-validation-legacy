using System.Collections.Generic;

namespace Better.Validation.EditorAddons.WindowModule.Pages
{
    public interface IWindowPage
    {
        public void Initialize();
        public IWindowPage DrawUpdate();
        public void Deconstruct();
    }
}