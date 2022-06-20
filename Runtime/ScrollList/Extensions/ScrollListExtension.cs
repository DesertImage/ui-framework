using UnityEngine;

namespace DesertImage.UI
{
    public class ScrollListExtension : MonoBehaviour, IScrollListExtension
    {
        [SerializeField] protected ScrollList scrollList;

        protected virtual void OnValidate()
        {
            if (scrollList) return;

            scrollList = GetComponent<ScrollList>();
        }

        protected virtual void OnDestroy()
        {
            scrollList = null;
        }
    }
}