using UnityEngine;

namespace DesertImage.Extensions
{
    public static class LayerExtensions
    {
        public static int GetLayerNumber(this LayerMask mask)
        {
            return (int) ( mask.value == 0 ? 0 : Mathf.Log((uint) mask.value, 2));
        }
    }
}