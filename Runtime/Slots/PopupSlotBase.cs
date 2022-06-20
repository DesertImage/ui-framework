namespace DesertImage.UI
{
    public class PopupSlotBase : ScreenSlotBase, IPopup
    {
        public bool DontHideIfNotForeground { get; }
        public WindowPriority Priority { get; }
        public bool IsPopup => true;
    }
}