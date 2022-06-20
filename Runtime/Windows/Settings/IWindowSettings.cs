namespace DesertImage.UI
{
    public interface IWindowSettings : IScreenSettings
    {
        bool DontHideIfNotForeground { get; }
        
        WindowPriority Priority { get; }
        bool IsPopup { get; }
    }
}