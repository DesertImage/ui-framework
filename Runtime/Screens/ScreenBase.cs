using System;
using UnityEngine;

namespace DesertImage.UI
{
    public class ScreenBase : MonoBehaviour, IScreen
    {
        public Action<IScreen> OnShowFinished { get; set; }
        public Action<IScreen> OnHideFinished { get; set; }
        public Action<IScreen> OnCloseRequest { get; set; }
        public Action<IScreen> OnDestroyed { get; set; }

        public virtual bool IsEnabled => gameObject.activeSelf;

        [SerializeField] protected ATransition enterTransition;
        [SerializeField] protected ATransition exitTransition;

        /// Calls automatically after ALayer.Register() by AUIManager
        public virtual void Initialize()
        {
            if (enterTransition)
            {
                enterTransition.Init(transform);
            }

            if (exitTransition)
            {
                exitTransition.Init(transform);
            }
        }

        protected virtual void Enable()
        {
            gameObject.SetActive(true);
        }

        protected virtual void Disable()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }

        public void SetEnterTransition(ITransition transition)
        {
            if (enterTransition == transition) return;

            if (enterTransition)
            {
                Destroy(enterTransition);
            }

            enterTransition = (ATransition)transition;
            enterTransition.Init(transform);
        }

        public void SetExitTransition(ITransition transition)
        {
            if (exitTransition == transition) return;

            if (exitTransition)
            {
                Destroy(exitTransition);
            }

            exitTransition = (ATransition)transition;
            exitTransition.Init(transform);
        }

        protected void Animate(ITransition transition, Action callback = null, bool animate = true,
            bool isVisible = true)
        {
            //TODO: refactor this. Don't like it
            if (enterTransition && enterTransition.IsInProcess /*&& enterTransition != (ATransition)transition*/)
            {
                enterTransition.Cancel();
            }

            if (exitTransition && exitTransition.IsInProcess && exitTransition != (ATransition)transition)
            {
                exitTransition.Cancel();
            }

            if (transition == null || !animate)
            {
                if (isVisible)
                {
                    if (transition != null)
                    {
                        transition.ShowHard(transform, callback);
                    }
                    else
                    {
                        callback?.Invoke();
                    }

                    Enable();
                }
                else
                {
                    callback?.Invoke();

                    Disable();
                }
            }
            else
            {
                Enable();

                transition.Play(transform, () =>
                {
                    callback?.Invoke();

                    if (isVisible) return;

                    Disable();
                });
            }
        }

        public virtual void Show()
        {
        }

        public virtual void Show<TSettings>(TSettings settings = default, bool animate = true)
            where TSettings : IScreenSettings
        {
        }

        public virtual void Hide(bool animate = true)
        {
            Animate
            (
                exitTransition,
                OnExitAnimationFinished,
                animate && (IsEnabled || enterTransition && enterTransition.IsInProcess),
                false
            );
        }

        protected virtual void UpdateHierarchy()
        {
            transform.SetAsLastSibling();
        }

        protected virtual void OnEnterAnimationFinished()
        {
            OnShowFinished?.Invoke(this);
        }

        protected virtual void OnExitAnimationFinished()
        {
            OnHideFinished?.Invoke(this);
        }
    }
}