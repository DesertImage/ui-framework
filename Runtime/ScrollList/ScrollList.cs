using UnityEngine;
using UnityEngine.EventSystems;

namespace DesertImage.UI
{
    public interface IScrollList
    {
        bool Horizontal { get; }
        bool Vertical { get; }

        Transform Content { get; }

        void Next(bool isLooped = false);
        void Previous(bool isLooped = false);

        void ScrollTo(int index);

        void Reset();
    }

    public abstract class ScrollList : MonoBehaviour, IScrollList, IDragHandler
    {
        public bool Horizontal => horizontal;
        public bool Vertical => vertical;

        public Transform Content => content;

        [SerializeField] private Transform content;

        [SerializeField] private bool horizontal = true;
        [SerializeField] private bool vertical;

        protected int CurrentElementIndex;

        public void Next(bool isLooped = false)
        {
            if (CurrentElementIndex + 1 >= content.childCount)
            {
                if (isLooped)
                {
                    CurrentElementIndex = 0;
                }
                else
                {
                    return;
                }
            }
            else
            {
                CurrentElementIndex++;
            }

            ScrollTo(CurrentElementIndex);
        }

        public void Previous(bool isLooped = false)
        {
            if (CurrentElementIndex <= 0)
            {
                if (isLooped)
                {
                    CurrentElementIndex = content.childCount - 1;
                }
                else
                {
                    return;
                }
            }
            else
            {
                CurrentElementIndex--;
            }

            ScrollTo(CurrentElementIndex);
        }

        public abstract void ScrollTo(int index);

        public virtual void Reset()
        {
            content.localPosition = Vector3.zero;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            content.localPosition += (Vector3)eventData.delta;
        }
    }
}