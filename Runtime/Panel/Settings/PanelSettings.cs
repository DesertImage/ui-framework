using System;

namespace DesertImage.UI.Panel
{
    [Serializable]
    public struct PanelSettings : IPanelSettings
    {
        public PanelSettings(PanelPriority priority)
        {
            Priority = priority;
        }

        public PanelPriority Priority { get; }
    }
}