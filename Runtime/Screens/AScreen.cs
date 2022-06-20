using UnityEngine;

namespace DesertImage.UI
{
    public abstract class AScreen<TId, TSettings> : ScreenBase, IScreen<TId, TSettings>
        where TSettings : IScreenSettings, new()
    {
        public virtual TId Id => default;

        public TSettings Settings => settings;

        [SerializeField] [Space(10)] protected TSettings settings;

        public override void Initialize()
        {
            base.Initialize();

            settings = new TSettings();
        }

        public override void Show()
        {
            base.Show();

            Show();
        }

        public override void Show<TScreenSettings>(TScreenSettings settings = default, bool animate = true)
        {
            base.Show(settings, animate);

            var screenSettings = settings as IScreenSettings;

            Show(settings == null ? default : (TSettings)screenSettings, animate);
        }

        public virtual void Show(TSettings settings = default, bool animate = true)
        {
            if (animate)
            {
                if (settings != null) Setup(settings);
            }

            Animate
            (
                enterTransition,
                OnEnterAnimationFinished,
                animate && (!IsEnabled || exitTransition && exitTransition.IsInProcess)
            );

            UpdateHierarchy();
        }

        public virtual void Setup(TSettings settings)
        {
            this.settings = settings;
        }
    }
}