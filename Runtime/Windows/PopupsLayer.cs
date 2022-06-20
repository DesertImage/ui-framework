using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DesertImage.UI
{
    public class PopupsLayer : MonoBehaviour
    {
        public Image BlackScreen => blackScreen;

        [SerializeField] protected Image blackScreen;

        private readonly List<GameObject> _containedScreens = new List<GameObject>();

        public void Register(IScreen screen)
        {
            var mono = screen as MonoBehaviour;

            if (!mono) return;

            mono.transform.SetParent(transform, false);

            _containedScreens.Add(mono.gameObject);
        }

        public virtual void ShowBlackScreen(bool animate = true)
        {
            if (!blackScreen) return;

            blackScreen.transform.SetAsFirstSibling();

            blackScreen.gameObject.SetActive(true);

            blackScreen.rectTransform.LeanCancel();
            blackScreen.rectTransform.LeanAlpha(.65f, .5f)
                .setFrom(blackScreen.color.a)
                .setEaseOutExpo()
                .setUseEstimatedTime(true);
        }

        public virtual void HideBlackScreen(bool animate = true)
        {
            if (!gameObject.activeSelf) return;

            if (!blackScreen) return;

            if (animate)
            {
                blackScreen.rectTransform.LeanCancel();
                blackScreen.rectTransform.LeanAlpha(0f, .2f)
                    .setOnComplete(() => blackScreen.gameObject.SetActive(false))
                    .setUseEstimatedTime(true);
            }
            else
            {
                blackScreen.gameObject.SetActive(false);
            }
        }

        public void RefreshBlackScreen()
        {
            foreach (var screen in _containedScreens)
            {
                if (!screen) continue;
                if (!screen.activeSelf) continue;

                ShowBlackScreen();

                return;
            }

            HideBlackScreen();
        }
    }
}