using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class GameObjectsExtensions
    {
        public static void SetActive(this GameObject[] gameObjects, bool isActive)
        {
            if (gameObjects == null) return;

            foreach (var gameObject in gameObjects)
            {
                if (!gameObject) continue;

                gameObject.SetActive(isActive);
            }
        }

        public static void SetActive<T>(this IEnumerable<T> objects, bool isActive = true) where T : MonoBehaviour
        {
            if (objects == null) return;

            foreach (var obj in objects)
            {
                obj.gameObject.SetActive(isActive);
            }
        }
    }
}