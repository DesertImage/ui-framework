using System;
using UnityEngine;

namespace DesertImage.UI
{
    public class ScreenSlotBase : MonoBehaviour, IScreenSlot
    {
        public Action<IScreen> OnShowFinished { get; set; }
        public Action<IScreen> OnHideFinished { get; set; }
        public Action<IScreen> OnCloseRequest { get; set; }
        public Action<IScreen> OnDestroyed { get; set; }

        public bool IsEnabled => gameObject.activeSelf;

        public IScreen Screen => screen;

        public bool IsIndividualInstance => isIndividualInstance;

        public virtual ushort Id { get; }

        [SerializeField] private ScreenBase screen;

        [SerializeField] [Space(7)] private bool isIndividualInstance;

        [SerializeField] [Space(15)] protected ATransition enterTransition;
        [SerializeField] protected ATransition exitTransition;

        public void Initialize()
        {
            screen.Initialize();
        }

        public virtual void SetScreenInstance(IScreen instance, bool dontSubscribe = false)
        {
            screen = instance as ScreenBase;

            if (dontSubscribe) return;

            RebindScreen();

            screen.OnShowFinished += OnScreenShowFinished;
            screen.OnHideFinished += OnScreenHideFinished;
            screen.OnCloseRequest += OnScreenCloseRequest;
            screen.OnDestroyed += OnScreenDestroyed;
        }

        public virtual void Show()
        {
            if (!isIndividualInstance)
            {
                RebindScreen();
            }

            screen.Show();
        }

        public virtual void Show<TSettings>(TSettings settings = default, bool animate = true)
            where TSettings : IScreenSettings
        {
            if (!isIndividualInstance)
            {
                RebindScreen();
            }

            screen.Show(settings, animate);
        }

        public virtual void Show(IScreenSettings settings)
        {
            if (!isIndividualInstance)
            {
                RebindScreen();
            }

            screen.Show(settings);
        }

        public virtual void Hide(bool animate)
        {
            screen.Hide(animate);
        }

        private void Unsubscribe(IScreen screen)
        {
            if (screen == null) return;

            screen.OnShowFinished -= OnScreenShowFinished;
            screen.OnHideFinished -= OnScreenHideFinished;

            screen.OnCloseRequest -= OnScreenCloseRequest;

            screen.OnDestroyed -= OnScreenDestroyed;
        }

        private void RebindScreen()
        {
            ResizeScreen();

            if (enterTransition)
            {
                screen.SetEnterTransition(enterTransition);
            }

            if (exitTransition)
            {
                screen.SetExitTransition(exitTransition);
            }
        }

        private void ResizeScreen()
        {
            var rectTransform = screen.transform as RectTransform;

            rectTransform.SetParent(transform);

            rectTransform.SetStretch();
        }

        private void OnScreenShowFinished(IScreen screen)
        {
            OnShowFinished?.Invoke(this);
        }

        private void OnScreenHideFinished(IScreen screen)
        {
            OnHideFinished?.Invoke(this);
        }

        private void OnScreenCloseRequest(IScreen screen)
        {
            OnCloseRequest?.Invoke(this);
        }

        private void OnScreenDestroyed(IScreen screen)
        {
            Unsubscribe(screen);

            OnDestroyed?.Invoke(this);
        }

        private void OnDestroy()
        {
            Unsubscribe(screen);
        }
    }
}