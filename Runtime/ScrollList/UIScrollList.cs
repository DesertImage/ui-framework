using UnityEngine;

namespace DesertImage.UI
{
    public class UIScrollList : ScrollList
    {
        public override void ScrollTo(int index)
        {
            var content = Content as RectTransform;
            
            if (index < 0 || index >= content.childCount) return;

            var targetChild = content.GetChild(index) as RectTransform;

            var contentPos = content.anchoredPosition;

            var targetPos = contentPos - (contentPos + targetChild.anchoredPosition);

            content.gameObject.LeanCancel();
            content.gameObject.LeanMoveLocal(targetPos, .6f).setEaseOutBack();
        }
    }
}