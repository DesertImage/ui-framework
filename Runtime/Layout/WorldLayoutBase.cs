using UnityEngine;

namespace DesertImage.UI
{
    public class WorldLayoutBase : LayoutBase
    {
        [SerializeField] private Vector3 direction = new Vector3(1f, 0f, 0f);

        [SerializeField] private bool IsStartFromZeroPoint;

        protected override void UpdatePosition(int index)
        {
            base.UpdatePosition(index);

            var child = content.GetChild(index);

            Vector3 newPos;

            var half = content.childCount / 2;
            var isEven = content.childCount % 2 == 0;

            if (IsStartFromZeroPoint)
            {
                newPos = direction * index * spacing;
            }
            else
            {
                newPos = direction * ((index - half) * spacing + (isEven ? spacing * .5f : 0f));
            }

            child.localPosition = newPos;

            OnUpdate();
        }
    }
}