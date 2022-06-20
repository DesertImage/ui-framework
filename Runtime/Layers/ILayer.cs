namespace DesertImage.UI
{
    public interface ILayer
    {
        void HideAll(bool animate = true);
    }

    public interface ILayer<in TId> : ILayer
    {
        void Show<TSettings>(TId id, TSettings settings = default) where TSettings : IScreenSettings;

        void HIde(TId id);

        IScreen Get(TId id);
    }

    public interface ILayer<in TId, TScreen> : ILayer<TId> where TScreen : IScreen
    {
        TScreen Current { get; }

        void Show<TSettings>(TScreen screen, TSettings settings = default) where TSettings : IScreenSettings;

        void Hide(TScreen screen, bool animate = true);

        void Register(TId id, TScreen screen);
        void Unregister(TId id, TScreen screen);
    }
}