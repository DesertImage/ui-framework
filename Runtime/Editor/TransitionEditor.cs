using DesertImage.UI;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CustomEditor(typeof(ATransition), true)]
    public class TransitionEditor : Editor
    {
        private ATransition _target;

        private void OnEnable()
        {
            _target = target as ATransition;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Play"))
            {
                _target.Play(_target.transform);    
            }
            
            base.OnInspectorGUI();
        }
    }
}