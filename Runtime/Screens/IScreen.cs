using System;

namespace DesertImage.UI
{
    public interface IScreen
    {
        Action<IScreen> OnShowFinished { get; set; }
        Action<IScreen> OnHideFinished { get; set; }

        Action<IScreen> OnCloseRequest { get; set; }

        Action<IScreen> OnDestroyed { get; set; }

        void Initialize();

        bool IsEnabled { get; }

        void Show();
        void Show<TSettings>(TSettings settings = default, bool animate = true) where TSettings : IScreenSettings;

        void Hide(bool animate = true);
    }

    public interface IScreen<TId> : IScreen
    {
        TId Id { get; }
    }

    public interface IScreen<TId, TSettings> : IScreen<TId>
    {
        TSettings Settings { get; }

        void Show(TSettings settings = default, bool animate = true);
    }
}