using UnityEngine;

namespace DesertImage.UI
{
    public struct GridData
    {
        public int Raw;
        public int Column;

        public int RawsCount;
        public int RawElementsCount;
        public int ChildCount;

        public int ElementsLeft;

        public int HorizontalHalf;
        public bool IsHorizontalEven;

        public int VerticalHalf;
        public bool IsVerticalEven;
    }

    public abstract class GridLayoutBase : LayoutBase
    {
        [SerializeField] protected float verticalSpacing = 10f;

        [SerializeField] [Space(15)] protected int columnsCount = 2;

        protected override void UpdatePosition(int index)
        {
            base.UpdatePosition(index);

            var rawsCount = content.childCount / columnsCount;

            if (content.childCount % columnsCount > 0)
            {
                rawsCount++;
            }

            var rawIndex = index / columnsCount;
            var columnIndex = index - rawIndex * columnsCount;

            var childCount = content.childCount;
            var elementsLeft = childCount - index;

            var rawElementsCount = Mathf.Clamp(childCount - rawIndex * columnsCount, 0, columnsCount);

            var horizontalHalf = (elementsLeft >= columnsCount ? columnsCount : rawElementsCount) / 2;

            var isHorizontalEven =
                columnsCount % 2 == 0 && elementsLeft >= columnsCount
                || rawElementsCount % 2 == 0;

            var verticalHalf = rawsCount / 2;
            var isVerticalEven = rawsCount % 2 == 0;

            var data = new GridData
            {
                Raw = rawIndex,
                Column = columnIndex,
                RawsCount = rawsCount,
                RawElementsCount = rawElementsCount,
                ChildCount = childCount,
                ElementsLeft = elementsLeft,
                HorizontalHalf = horizontalHalf,
                IsHorizontalEven = isHorizontalEven,
                VerticalHalf = verticalHalf,
                IsVerticalEven = isVerticalEven,
            };

            GetPosition(index, data);

            OnUpdate();
        }

        protected abstract void GetPosition(int childIndex, GridData data);
    }
}