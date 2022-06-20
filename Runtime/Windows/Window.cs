namespace DesertImage.UI
{
    public class Window : Window<WindowSettings>
    {
    }

    public class Window<TSettings> : AScreen<ushort, TSettings>, IWindow where TSettings : IWindowSettings, new()
    {
        public virtual bool DontHideIfNotForeground => settings?.DontHideIfNotForeground ?? false;
        public virtual WindowPriority Priority { get; protected set; }
        public virtual bool IsPopup => settings?.IsPopup ?? false;

        public override void Setup(TSettings settings)
        {
            base.Setup(settings);

            Priority = settings.Priority;
        }

        public virtual void Close()
        {
            OnCloseRequest?.Invoke(this);
        }
    }
}