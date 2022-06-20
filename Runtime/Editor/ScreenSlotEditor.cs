using DesertImage.UI;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScreenSlotBase), true)]
    public class ScreenSlotEditor : Editor
    {
        private static ScreenSlotBase _target;

        private void OnEnable()
        {
            _target = (ScreenSlotBase)target;
        }

        public override void OnInspectorGUI()
        {
            var isPreviewAlready = _target.transform.childCount == 1 &&
                                   _target.transform.GetChild(0).GetComponent<ScreenBase>();

            if (GUILayout.Button(isPreviewAlready ? "Unpreview" : "Preview"))
            {
                SwitchPreview(_target);
            }

            GUILayout.Space(15);

            base.OnInspectorGUI();
        }

        public static void SwitchPreview(ScreenSlotBase slot)
        {
            var isPreviewAlready = slot.transform.childCount == 1 &&
                                   slot.transform.GetChild(0).GetComponent<ScreenBase>();

            if (isPreviewAlready)
            {
                DestroyImmediate(slot.transform.GetChild(0).gameObject);

                return;
            }

            Preview(slot);
        }

        public static void Preview(ScreenSlotBase slot)
        {
            var screenObj =
                PrefabUtility.InstantiatePrefab(slot.Screen as MonoBehaviour, slot.transform);

            var monoBehaviour = screenObj as MonoBehaviour;

            monoBehaviour.tag = "EditorOnly";

            var rect = monoBehaviour.transform as RectTransform;

            rect.transform.localScale = Vector3.one;

            rect.anchoredPosition = new Vector2();

            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(1f, 1f);

            rect.pivot = new Vector2(0.5f, 0.5f);

            rect.offsetMin = rect.offsetMax = new Vector2(0, 0);
        }
    }
}