using UnityEngine;
using UnityEngine.Serialization;

namespace DesertImage.UI
{
    public abstract class UISetupBase : ScriptableObject, IUISetup
    {
        [FormerlySerializedAs("uiManagerPrefab")] [SerializeField]
        protected UIManager uiManager;

#if UNITY_EDITOR
        [SerializeField] [Space(10)] private Sprite maket;
#endif

        public abstract IUIManager Setup(bool isDebugMode = false);
    }
}