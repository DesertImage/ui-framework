using UnityEngine;
using UnityEngine.EventSystems;

namespace DesertImage.UI
{
    public class WorldScrollList : ScrollList, IBeginDragHandler
    {
        private Camera _camera;

        protected Vector3 StartDragPos;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public override void ScrollTo(int index)
        {
            if (index < 0 || !Content || index >= Content.childCount) return;

            var targetChild = Content.GetChild(index);

            var localPosition = Content.localPosition;
            var targetPos = localPosition - (localPosition + targetChild.localPosition);

            Content.gameObject.LeanCancel();
            Content.gameObject.LeanMoveLocal(targetPos, .6f).setEaseOutBack();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            StartDragPos = _camera.ScreenToWorldPoint(eventData.position);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            var difference = _camera.ScreenToWorldPoint(eventData.position) - StartDragPos;

            Content.position = transform.position + difference;
        }
    }
}