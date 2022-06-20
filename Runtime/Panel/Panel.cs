namespace DesertImage.UI.Panel
{
    public class Panel : Panel<PanelSettings>
    {
        protected override void UpdateHierarchy()
        {
        }
    }

    public class Panel<TSettings> : AScreen<ushort, TSettings>, IPanel where TSettings : IPanelSettings, new()
    {
        public bool IsShowing { get; protected set; }

        public virtual PanelPriority Priority { get; private set; }

        public override void Show(TSettings settings = default, bool animate = true)
        {
            base.Show(settings, animate);

            IsShowing = true;
        }

        public override void Setup(TSettings settings)
        {
            base.Setup(settings);

            Priority = settings.Priority;
        }


        public override void Hide(bool animate = true)
        {
            base.Hide(animate);

            IsShowing = false;
        }
    }
}