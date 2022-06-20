namespace DesertImage.UI.Panel
{
    public interface IPanelSettings : IScreenSettings
    {
        PanelPriority Priority { get; }
    }
}