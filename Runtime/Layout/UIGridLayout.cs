using UnityEngine;

namespace DesertImage.UI
{
    public class UIGridLayout : GridLayoutBase
    {
        protected override void GetPosition(int childIndex, GridData data)
        {
            var childRect = content.GetChild(childIndex) as RectTransform;

            var columnIndex = childIndex - data.Raw * columnsCount;

            childRect.anchoredPosition = new Vector2
            (
                (columnIndex - data.HorizontalHalf) * spacing + (data.IsHorizontalEven ? spacing * .5f : 0f),
                (data.RawsCount - 1 - data.Raw - data.VerticalHalf) * verticalSpacing +
                (data.IsVerticalEven ? verticalSpacing * .5f : 0f)
            );
        }
    }
}