namespace DesertImage.UI
{
    public interface IPopup : IWindow
    {
    }

    public interface IPopup<TId> : IScreen<TId>, IPopup
    {
        WindowPriority Priority { get; }
    }
}