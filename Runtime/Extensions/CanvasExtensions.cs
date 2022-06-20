using UnityEngine;
using UnityEngine.UI;

namespace DesertImage.Extensions
{
    public static partial class CanvasExtensions
    {
        private static Camera MainCamera
        {
            get
            {
                if (!_mainCamera) _mainCamera = Camera.main;

                return _mainCamera;
            }
        }

        private static Camera _mainCamera;

        public static Vector3 WorldToUISpace(this Canvas parentCanvas, Vector3 worldPos)
        {
            //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
            var screenPos = MainCamera.WorldToScreenPoint(worldPos);

            //Convert the screenpoint to ui rectangle local point
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos,
                parentCanvas.worldCamera, out var movePos);
            //Convert the local point to world point
            return parentCanvas.transform.TransformPoint(movePos);
        }

        public static Vector3 WorldToUISpace(this Canvas parentCanvas, Camera camera, Vector3 worldPos)
        {
            //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
            var screenPos = camera.WorldToScreenPoint(worldPos);

            //Convert the screenpoint to ui rectangle local point
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos,
                camera, out var movePos);
            //Convert the local point to world point
            return parentCanvas.transform.TransformPoint(movePos);
        }

        public static void SetAlpha(this Image image, float alpha)
        {
            var color = image.color;

            color.a = alpha;

            image.color = color;
        }
    }
}