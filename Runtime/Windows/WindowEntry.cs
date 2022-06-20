namespace DesertImage.UI
{
    public struct WindowEntry
    {
        public readonly IWindow Window;
        public readonly IWindowSettings Settings;

        public WindowEntry(IWindow window, IWindowSettings settings)
        {
            Window = window;
            Settings = settings;
        }

        public void Show(bool animate = true)
        {
            Window.Show(Settings, animate);
        }
    }
}