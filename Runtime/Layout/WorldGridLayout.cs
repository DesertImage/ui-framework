using UnityEngine;

namespace DesertImage.UI
{
    public class WorldGridLayout : GridLayoutBase
    {
        protected override void GetPosition(int childIndex, GridData data)
        {
            var child = content.GetChild(childIndex);

            var columnIndex = childIndex - data.Raw * columnsCount;

            child.localPosition = new Vector3
            (
                (columnIndex - data.HorizontalHalf) * spacing + (data.IsHorizontalEven ? spacing * .5f : 0f),
                (data.RawsCount - 1 - data.Raw - data.VerticalHalf) * verticalSpacing +
                (data.IsVerticalEven ? verticalSpacing * .5f : 0f)
            );
        }
    }
}