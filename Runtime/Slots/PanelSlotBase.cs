using DesertImage.UI.Panel;
using UnityEngine;

namespace DesertImage.UI
{
    public class PanelSlotBase : ScreenSlotBase, IPanel
    {
        public bool IsShowing { get; private set; }
        public PanelPriority Priority => priority;

        [SerializeField] [Space(15)] private PanelPriority priority;

        public override void Show<TSettings>(TSettings settings = default, bool animate = true)
        {
            base.Show(settings, animate);

            IsShowing = true;
        }

        public override void Hide(bool animate)
        {
            base.Hide(animate);

            IsShowing = false;
        }

        private void OnValidate()
        {
            if (!(Screen is IPanel screen)) return;

            if (priority != default) return;

            priority = screen.Priority;
        }
    }
}