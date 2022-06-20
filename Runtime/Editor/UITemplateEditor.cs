using DesertImage.UI;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UITemplate))]
    public class UITemplateEditor : Editor
    {
        private static UITemplate _target;

        private void OnEnable()
        {
            _target = (UITemplate)target;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Preview"))
            {
                foreach (var slot in _target.Slots)
                {
                    var screenSlotBase = slot as ScreenSlotBase;

                    if (screenSlotBase.transform.childCount > 0) continue;

                    ScreenSlotEditor.Preview(slot as ScreenSlotBase);
                }

                EditorUtility.SetDirty(_target);
            }

            if (GUILayout.Button("Unpreview"))
            {
                foreach (var slot in _target.Slots)
                {
                    var screenSlotBase = slot as ScreenSlotBase;

                    if (screenSlotBase.transform.childCount == 0) continue;

                    ScreenSlotEditor.SwitchPreview(slot as ScreenSlotBase);
                }

                EditorUtility.SetDirty(_target);
            }

            GUILayout.Space(15);

            base.OnInspectorGUI();
        }
    }
}