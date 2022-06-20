using System.Collections.Generic;
using DesertImage.UI.Panel;
using UnityEngine;

namespace DesertImage.UI
{
    public class UIManager : AUIManager<ushort>
    {
        [SerializeField] private WindowsLayer windowsLayer;
        [SerializeField] private PanelsLayer panelsLayer;

        private readonly Dictionary<ushort, ILayer<ushort>> _screens = new Dictionary<ushort, ILayer<ushort>>();

        public override void Show<TSettings>(ushort id, TSettings settings = default)
        {
            if (!_screens.TryGetValue(id, out var layer))
            {
#if UNITY_EDITOR
                Debug.LogError($"[UIManager]: There is not such screen with id {id}");
#endif
                return;
            }

            layer.Show(id, settings);
        }

        public override void ShowAll(bool animate = true)
        {
            foreach (var pair in _screens)
            {
                Show<IScreenSettings>(pair.Key);
            }
        }

        public override void Hide(ushort id)
        {
            if (_screens.TryGetValue(id, out var layer))
            {
                layer.HIde(id);
            }
        }

        public override void HideAll(bool animate = true)
        {
            windowsLayer.HideAll();
            // panelsLayer.HideAll();
            // foreach (var layer in _screens.Values)
            // {
            // layer.HideAll();
            // }
        }

        public override IScreen Get(ushort id)
        {
            _screens.TryGetValue(id, out var layer);

            var screen = layer?.Get(id);

            return screen is IScreenSlot screenSlot ? screenSlot.Screen : screen;
        }

        public override void Back()
        {
            base.Back();

            if (windowsLayer.History.Count <= 1) return;

            windowsLayer.Hide(windowsLayer.Current);
        }

        public override void Register<TScreen>(ushort id, TScreen screen)
        {
            ILayer<ushort> layer = null;

            switch (screen)
            {
                case IWindow window:
                    windowsLayer.Register(id, window);

                    layer = windowsLayer;

                    break;

                case IPanel panel:
                    panelsLayer.Register(id, panel);

                    layer = panelsLayer;

                    break;
            }

            if (layer == null) return;

            if (_screens.ContainsKey(id)) return;

            if (screen is IScreen scr)
            {
                scr.Initialize();
            }

            _screens.Add(id, layer);
        }

        public override void Unregister<TScreen>(ushort id, TScreen screen)
        {
            switch (screen)
            {
                case IWindow window:
                    windowsLayer.Unregister(id, window);
                    break;

                case IPanel panel:
                    panelsLayer.Unregister(id, panel);
                    break;
            }

            _screens.Remove(id);
        }
    }
}