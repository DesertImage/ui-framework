#if UNITY_EDITOR
// using Unity.DeviceSimulator;
#endif
using UIFramework;
using UnityEngine;

namespace DesertImage.UI
{
    [ExecuteInEditMode]
    public class AdaptiveSafeArea : MonoBehaviour
    {
        private static Rect EditorSafeArea = new Rect(132 * .7f, 63 * .7f, 2172 * 1.3f, 1062 * 1.3f);

        private RectTransform _safeAreaRect;
        private Canvas _canvas;
        private Rect _lastSafeArea;

        private Camera _camera;

        private void OnEnable()
        {
#if UNITY_EDITOR
            // DeviceSimulatorCallbacks.OnDeviceChange += DeviceSimulatorCallbacksOnOnDeviceChange;
#endif
        }

        private void Awake()
        {
            _camera = Camera.main;

            _safeAreaRect = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
        }

        private void Start()
        {
            Resize();
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            // DeviceSimulatorCallbacks.OnDeviceChange -= DeviceSimulatorCallbacksOnOnDeviceChange;
#endif
        }

        public void Resize(Rect safeArea)
        {
#if UNITY_EDITOR
            Awake();
#endif
            var screenResolution = GetScreenResolution();

            if (safeArea == _lastSafeArea || !_canvas) return;

            _lastSafeArea = safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            // var resolution =
            //     useCameraResolution
            //         ? new Vector2(_camera.pixelWidth, _camera.pixelHeight)
            //         : new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

            anchorMin.x /= screenResolution.x;
            anchorMin.y /= screenResolution.y;

            anchorMax.x /= screenResolution.x;
            anchorMax.y /= screenResolution.y;

            _safeAreaRect.anchorMin = anchorMin;
            _safeAreaRect.anchorMax = anchorMax;
        }

#if UNITY_EDITOR
        [ContextMenu("Resize")]
#endif
        private void Resize()
        {
            Resize(GetSafeArea());
        }

        private Rect GetSafeArea()
        {
            var resolution = GetScreenResolution();

            EditorSafeArea = resolution.x < resolution.y
                ? new Rect(0f, 102, 1125, 2202)
                : new Rect(132, 63, 2172, 1062);

#if UNITY_EDITOR
            return DeviceSimulatorTools.IsInSimulatorMode() || Application.isPlaying ? Screen.safeArea : EditorSafeArea;
#endif
            return Screen.safeArea;
        }

        private Vector2 GetScreenResolution()
        {
#if UNITY_EDITOR
            var isSimulator = DeviceSimulatorTools.IsInSimulatorMode();

            if (Application.isPlaying)
            {
                return isSimulator
                    ? new Vector2(Screen.currentResolution.width, Screen.currentResolution.height)
                    : new Vector2(Screen.width, Screen.height);
            }

            return isSimulator || !_camera
                ? new Vector2(Screen.currentResolution.width, Screen.currentResolution.height)
                : new Vector2(_camera.pixelWidth, _camera.pixelHeight);
#endif
            return new Vector2(Screen.width, Screen.height);
        }

        private void OnRectTransformDimensionsChange()
        {
#if !UNITY_EDITOR
            Resize();
#endif
        }

#if UNITY_EDITOR
        private void DeviceSimulatorCallbacksOnOnDeviceChange()
        {
            Resize();
        }
#endif
    }
}