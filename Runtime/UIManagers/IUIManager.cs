namespace DesertImage.UI
{
    public interface IUIManager
    {
        void ShowAll(bool animate = true);

        void HideAll(bool animate = true);
    }

    public interface IUIManager<in TId> : IUIManager
    {
        void Show<TSettings>(TId id, TSettings settings = default) where TSettings : IScreenSettings;

        void Hide(TId id);

        void Register<TScreen>(TId id, TScreen screen) where TScreen : IScreen;
        void Unregister<TScreen>(TId id, TScreen screen) where TScreen : IScreen;

        IScreen Get(TId id);
    }
}