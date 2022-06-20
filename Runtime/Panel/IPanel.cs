namespace DesertImage.UI.Panel
{
    public interface IPanel : IScreen
    {
        bool IsShowing { get; }

        PanelPriority Priority { get; }
    }

    public interface IPanel<TId> : IScreen<TId>, IPanel
    {
    }
}