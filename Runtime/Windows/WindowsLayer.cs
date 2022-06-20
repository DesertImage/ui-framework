using System;
using System.Collections.Generic;
using UnityEngine;

namespace DesertImage.UI
{
    public class WindowsLayer : Layer<ushort, IWindow>
    {
        public event Action OnBlockScreen = delegate { };
        public event Action OnUnblockScreen = delegate { };

        [SerializeField] private PopupsLayer popupsLayer;

        public readonly Stack<WindowEntry> History = new Stack<WindowEntry>();

        private readonly Queue<WindowEntry> _queue = new Queue<WindowEntry>();

        private readonly HashSet<IWindow> _transitions = new HashSet<IWindow>();

        private bool TransitionsInProgress => _transitions.Count > 0;

        private void Awake()
        {
            popupsLayer.HideBlackScreen();
        }

        #region SHOW

        public override void Show<TSettings>(IWindow screen, TSettings settings = default)
        {
            if (ShouldEnqueue(screen, settings as IWindowSettings))
            {
#if DEBUG
                UnityEngine.Debug.LogError($"<b>[WindowsLayer]</b> ENQUEUEUE {screen}");
#endif

                _queue.Enqueue(new WindowEntry(screen, settings as IWindowSettings));
            }
            else
            {
                ShowProcess(new WindowEntry(screen, settings as IWindowSettings));
            }
        }

        private void ShowProcess(WindowEntry entry, bool animate = true)
        {
            if (entry.Window == Current)
            {
                entry.Show();

                return;
            }

            if (Current is { DontHideIfNotForeground: false } && !entry.Window.IsPopup)
            {
                Current.Hide();
            }

            entry.Show(animate);

            History.Push(entry);

            if (!entry.Window.IsPopup && Current is { IsPopup: true }) return;

            AddTransition(entry.Window);

            if (entry.Window.IsPopup)
            {
                if (entry.Settings is IPopupSettings popupSettings)
                {
                    popupsLayer.BlackScreen.color = popupSettings.BlackScreenColor;
                }
                else
                {
                    popupsLayer.BlackScreen.color = new Color(0f, 0f, 0f, .6f);
                }

                popupsLayer.ShowBlackScreen();
            }
            else
            {
                popupsLayer.HideBlackScreen();
            }

            Current = entry.Window;
        }

        #endregion

        #region HIDE

        public override void Hide(IWindow screen, bool animate = true)
        {
            if (screen != Current && Current is { IsEnabled: true }) return;

            if (Current == null)
            {
                screen.Hide(animate);

                return;
            }

            History.Pop();

            AddTransition(screen);

            screen.Hide();

            if (Current?.IsPopup ?? false)
            {
                popupsLayer.HideBlackScreen();
            }

            var lastScreen = Current;

            Current = null;

            if (_queue.Count > 0)
            {
                ShowNext();
            }
            else if ((lastScreen?.IsPopup ?? false) && lastScreen != screen)
            {
                if (History.Count > 0)
                {
                    var entry = History.Peek();

                    if (entry.Window.IsPopup)
                    {
                        ShowPrevious(!screen.IsPopup);
                    }
                }
            }
        }

        public override void HideAll(bool animate = true)
        {
            base.HideAll(animate);

            Current = null;

            History.Clear();
        }

        #endregion

        #region QUEUE / HISTORY

        private bool ShouldEnqueue(IWindow window, IWindowSettings settings)
        {
            if (Current == null && _queue.Count == 0)
            {
                return false;
            }

            //TODO: look wisely at second part of condition - it cause problems
            return window.Priority != WindowPriority.Foreground &&
                   (Current != null && Current.IsPopup && window.IsPopup);
        }

        private void ShowPrevious(bool animate = true)
        {
            if (History.Count == 0) return;

            ShowProcess(History.Pop(), animate);
        }

        private void ShowNext()
        {
            if (_queue.Count == 0) return;

            ShowProcess(_queue.Dequeue());
        }

        #endregion

        #region TRANSITIONS

        private void AddTransition(IWindow window)
        {
            _transitions.Add(window);

            OnBlockScreen?.Invoke();
        }

        private void RemoveTransition(IWindow window)
        {
            _transitions.Remove(window);

            if (TransitionsInProgress) return;

            OnUnblockScreen?.Invoke();
        }

        #endregion

        #region REGISTER / UNREGISTER

        protected override void RegisterProcess(ushort id, IWindow screen)
        {
            base.RegisterProcess(id, screen);

            if (screen.IsPopup)
            {
                popupsLayer.Register(screen);
            }

            screen.OnShowFinished += OnScreenEnterAnimationFinished;
            screen.OnHideFinished += OnScreenExitAnimationFinished;
        }

        protected override void UnregisterProcess(ushort id, IWindow screen)
        {
            base.UnregisterProcess(id, screen);

            screen.OnShowFinished -= OnScreenEnterAnimationFinished;
            screen.OnHideFinished -= OnScreenExitAnimationFinished;
        }

        #endregion

        #region CALLBACKS

        private void OnScreenEnterAnimationFinished(IScreen screen)
        {
            RemoveTransition((IWindow)screen);
        }

        private void OnScreenExitAnimationFinished(IScreen screen)
        {
            RemoveTransition((IWindow)screen);

            if (Current?.IsPopup ?? false)
            {
                popupsLayer.ShowBlackScreen();
            }
            else
            {
                popupsLayer.HideBlackScreen();
            }
        }

        #endregion
    }
}