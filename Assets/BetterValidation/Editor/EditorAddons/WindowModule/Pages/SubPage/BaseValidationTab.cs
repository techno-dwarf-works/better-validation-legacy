using System.Collections.Generic;

namespace Better.Validation.EditorAddons.WindowModule.Pages.SubPage
{
    public abstract class BaseValidationTab : IValidationTab
    {
        protected ValidatorCommands _commands;
        public abstract int Order { get; }
        public abstract string GetTabName();

        public virtual void Initialize()
        {
            _commands = new ValidatorCommands();
        }

        public abstract List<ValidationCommandData> DrawUpdate();
    }
}