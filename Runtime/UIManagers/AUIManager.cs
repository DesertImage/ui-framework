using UnityEngine;
using UnityEngine.UI;

namespace DesertImage.UI
{
    [RequireComponent(typeof(Canvas))]
    public abstract class AUIManager<TId> : MonoBehaviour, IUIManager<TId>
    {
        public static Canvas UICanvas { get; private set; }
        public static RectTransform AdaptiveSafeArea { get; private set; }

        protected GraphicRaycaster GraphicRaycaster;

        private void Awake()
        {
            UICanvas = GetComponent<Canvas>();
            AdaptiveSafeArea = GetComponentInChildren<AdaptiveSafeArea>().transform as RectTransform;
            GraphicRaycaster = GetComponent<GraphicRaycaster>();
        }

        public abstract void Show<TSettings>(TId id, TSettings settings = default) where TSettings : IScreenSettings;
        public abstract void ShowAll(bool animate = true);

        public abstract void Hide(TId id);

        public abstract void HideAll(bool animate = true);

        public abstract void Register<TScreen>(TId id, TScreen screen) where TScreen : IScreen;

        public abstract void Unregister<TScreen>(TId id, TScreen screen) where TScreen : IScreen;

        public virtual IScreen Get(TId id)
        {
            return default;
        }

        public virtual void Back()
        {
        }

        protected virtual void BlockScreen()
        {
            if (GraphicRaycaster == null) return;

            GraphicRaycaster.enabled = false;
        }

        protected virtual void UnblockScreen()
        {
            if (GraphicRaycaster == null) return;

            GraphicRaycaster.enabled = true;
        }
    }
}