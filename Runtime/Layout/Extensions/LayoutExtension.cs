using UnityEngine;

namespace DesertImage.UI
{
    public interface ILayoutExtension
    {
    }

    [RequireComponent(typeof(UILayoutBase))]
    public abstract class LayoutExtension : MonoBehaviour, ILayoutExtension
    {
        [SerializeField] protected LayoutBase layout;

        private void Awake()
        {
            if (!layout)
            {
                layout = GetComponent<LayoutBase>();
            }
        }

        private void OnEnable()
        {
            layout.OnPreUpdate += OnLayoutPreUpdate;
        }

        private void OnDisable()
        {
            if (!layout) return;

            layout.OnPreUpdate -= OnLayoutPreUpdate;
        }

        protected abstract void OnLayoutPreUpdate();

        private void OnValidate()
        {
            if (layout) return;

            layout = GetComponent<LayoutBase>();
        }

        private void OnDestroy()
        {
            layout = null;
        }
    }
}