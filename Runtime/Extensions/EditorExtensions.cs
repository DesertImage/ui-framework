using DesertImage.Extensions;
using UnityEngine;

namespace Extensions
{
    public static class EditorExtensions
    {
        public static void FindInChildsIfNull(this Transform parent, ref Transform transform, string name,
            bool matchCase = false)
        {
            if (transform || !parent) return;

            transform = parent.FindChildWithName(name, matchCase);
        }
    }
}