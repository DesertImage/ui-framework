using System.Collections.Generic;
using UnityEngine;

namespace DesertImage.UI
{
    [CreateAssetMenu(fileName = "UISetup", menuName = "UI/UITemplate")]
    public class UITemplateSetup : UISetupBase
    {
        [SerializeField] private UITemplate uiTemplate;

#if UNITY_IOS || UNITY_EDITOR
        [SerializeField] private UITemplate iPadTemplate;
#endif
#if UNITY_EDITOR
        /// <summary>
        /// Execute iPad mode even on Android
        /// </summary>
        [SerializeField] private bool simulatePad;
#endif
        public override IUIManager Setup(bool isDebugMode = false)
        {
            var uiManager = Instantiate(this.uiManager);

            var cachedScreens = new Dictionary<int, IScreen>();

            var template = uiTemplate;

#if UNITY_EDITOR
            template = simulatePad && iPadTemplate ? iPadTemplate : uiTemplate;
#elif UNITY_IOS
            var isPad = UnityEngine.iOS.Device.generation.ToString().Contains("iPad");

            template = isPad && iPadTemplate ? iPadTemplate : uiTemplate;
#endif
            foreach (var screenSlot in template.Slots)
            {
                var slot = Instantiate(screenSlot as ScreenSlotBase, UIManager.AdaptiveSafeArea);

                var targetScreen = slot.Screen as ScreenBase;

                var screen = slot.Screen;

                if (slot.IsIndividualInstance || !cachedScreens.TryGetValue(targetScreen.GetInstanceID(), out screen))
                {
                    screen = Instantiate(targetScreen, slot.transform);

                    if (!slot.IsIndividualInstance)
                    {
                        cachedScreens.Add(targetScreen.GetInstanceID(), screen);
                    }
                }

                slot.SetScreenInstance(screen);

                uiManager.Register(slot.Id, slot);
            }

            return uiManager;
        }
    }
}