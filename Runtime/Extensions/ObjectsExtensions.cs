using System.Collections.Generic;
using UnityEngine;

namespace DesertImage
{
    public static class ObjectsExtensions
    {
        public static void SetEnabled(this IEnumerable<Renderer> objects, bool isEnabled = true)
        {
            foreach (var obj in objects)
            {
                obj.enabled = isEnabled;
            }
        }
    }
}