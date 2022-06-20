using System;
using UnityEngine;

namespace DesertImage.UI
{
    public abstract class LayoutBase : MonoBehaviour, ILayout
    {
        public Action OnPreUpdate { get; set; } = delegate { };
        public Action OnUpdate { get; set; } = delegate { };

        public Transform Content => content;
        public float Spacing => spacing;

        [SerializeField] protected Transform content;
        [SerializeField] [Space(15)] protected float spacing = 10f;

        public void Align()
        {
            if (!content) return;

            if (content.childCount == 0) return;

            for (var i = 0; i < content.childCount; i++)
            {
                UpdatePosition(i);
            }
        }

        protected virtual void UpdatePosition(int index)
        {
            OnPreUpdate();
        }

        protected virtual void OnValidate()
        {
            if (Application.isPlaying) return;

            Align();

            if (content) return;

            content = transform;
        }

        protected virtual void OnDestroy()
        {
            content = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;

            Align();
        }
#endif
    }
}