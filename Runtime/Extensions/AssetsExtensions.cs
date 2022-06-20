using System.Collections.Generic;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Extensions
{
    public static class AssetsExtensions
    {
        public static T[] GetAllAssetsByType<T>(this object sender, string[] searchInPath = null) where T : Object
        {
#if UNITY_EDITOR

            var guids = searchInPath == null
                ? AssetDatabase.FindAssets($"t:{typeof(T)}")
                : AssetDatabase.FindAssets($"t:{typeof(T)}", searchInPath);

            var assets = new List<T>();

            foreach (var guid in guids)
            {
                assets.Add(AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)));
            }

            return assets.ToArray();
#else
        return null;
#endif
        }
    }
}