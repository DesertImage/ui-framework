using UnityEngine;

namespace DesertImage.UI
{
    [RequireComponent(typeof(LayoutBase))]
    public class AutoAlignLayout : MonoBehaviour
    {
        private LayoutBase _layout;

        private void Awake()
        {
            _layout = GetComponent<LayoutBase>();
        }

        private void LateUpdate()
        {
            if (!_layout) return;

            _layout.Align();
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (!_layout)
            {
                _layout = GetComponent<LayoutBase>();

                return;
            }

            _layout.Align();
        }
#endif
    }
}