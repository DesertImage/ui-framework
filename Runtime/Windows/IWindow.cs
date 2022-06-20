namespace DesertImage.UI
{
    public interface IWindow : IScreen
    {
        bool DontHideIfNotForeground { get; }
        WindowPriority Priority { get; }
        bool IsPopup { get; }
    }

    public interface IWindow<TId> : IScreen<TId>, IWindow
    {
    }
}