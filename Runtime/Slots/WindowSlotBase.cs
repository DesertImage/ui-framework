using UnityEngine;

namespace DesertImage.UI
{
    public class WindowSlotBase : ScreenSlotBase, IWindow
    {
        public bool DontHideIfNotForeground => dontHideIfNotForeground;

        public WindowPriority Priority { get; }
        public bool IsPopup => false;

        [SerializeField] [Space(15)] private WindowPriority priority;
        [SerializeField] private bool dontHideIfNotForeground;

        private void OnValidate()
        {
            if (!(Screen is IWindow screen)) return;

            if (priority != default) return;

            priority = screen.Priority;
            dontHideIfNotForeground = screen.DontHideIfNotForeground;
        }
    }
}