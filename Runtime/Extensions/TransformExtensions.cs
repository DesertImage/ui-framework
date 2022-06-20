using UnityEngine;

namespace DesertImage.Extensions
{
    public static class TransformExtensions
    {
        public static Transform FindChildWithName(this Transform parent, string name, bool matchCase = false)
        {
            (bool, Transform) FindInChild(Transform transform, string targetName)
            {
                if (transform.name.Contains(targetName))
                {
                    if (matchCase)
                    {
                        if (string.Equals(transform.name, targetName))
                        {
                            return (true, transform);
                        }
                    }
                    else
                    {
                        return (true, transform);
                    }
                }

                if (!transform || transform.childCount == 0) return (false, null);

                for (var i = 0; i < transform.childCount; i++)
                {
                    var result = FindInChild(transform.GetChild(i), targetName);

                    if (!result.Item1) continue;

                    return result;
                }

                return (false, null);
            }

            return parent ? FindInChild(parent, name).Item2 : default;
        }
    }
}