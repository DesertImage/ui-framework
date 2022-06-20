using System;
using UnityEngine;

namespace DesertImage.UI
{
    [Serializable]
    public class WindowSettings : IWindowSettings
    {
        public WindowSettings(bool dontHideIfNotForeground, WindowPriority priority = WindowPriority.Foreground, bool isPopup = false)
        {
            this.dontHideIfNotForeground = dontHideIfNotForeground;
            this.priority = priority;
            this.isPopup = isPopup;
        }

        public bool DontHideIfNotForeground => dontHideIfNotForeground;

        public WindowPriority Priority => priority;
        public bool IsPopup => isPopup;

        [SerializeField] private bool dontHideIfNotForeground;

        [SerializeField] private WindowPriority priority;

        [SerializeField] private bool isPopup;

        public WindowSettings()
        {
            
        }
    }
}