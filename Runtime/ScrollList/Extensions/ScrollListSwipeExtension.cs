using UnityEngine;
using UnityEngine.EventSystems;

namespace DesertImage.UI
{
    public class ScrollListSwipeExtension : ScrollListExtension, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private float minDistanceToSwipe = 80;

        private Vector2 _startDragPos;

        private void OnEnable()
        {
            scrollList.enabled = false;
        }

        private void OnDisable()
        {
            if (!scrollList) return;

            scrollList.enabled = true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startDragPos = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var delta = eventData.position - _startDragPos;

            if (delta.magnitude < minDistanceToSwipe) return;

            if (scrollList.Vertical)
            {
                if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
                {
                    if (delta.y > 0f)
                    {
                        scrollList.Next();
                    }
                    else
                    {
                        scrollList.Previous();
                    }
                }
            }
            else
            {
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    if (delta.x > 0f)
                    {
                        scrollList.Previous();
                    }
                    else
                    {
                        scrollList.Next();
                    }
                }
            }
        }
    }
}