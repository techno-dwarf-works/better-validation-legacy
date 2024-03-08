namespace Better.Validation.EditorAddons.WindowModule
{
    public interface IWindowPage
    {
        public void Initialize();
        public IWindowPage DrawUpdate();
        public void Deconstruct();
    }
}