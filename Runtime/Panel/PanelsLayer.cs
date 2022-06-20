using UnityEngine;

namespace DesertImage.UI.Panel
{
    public class PanelsLayer : Layer<ushort, IPanel>
    {
        [SerializeField] private PriorityPanelsLayer[] priorityLayers;

        public override void Hide(IPanel screen, bool animate = true)
        {
            if (!screen.IsShowing) return;

            base.Hide(screen, animate);
        }

        protected override void RegisterProcess(ushort id, IPanel screen)
        {
            base.RegisterProcess(id, screen);

            var newParent = transform;

            foreach (var priorityLayer in priorityLayers)
            {
                if (priorityLayer.Priority != screen.Priority) continue;

                newParent = priorityLayer.Parent;

                break;
            }

            (screen as MonoBehaviour)?.transform.SetParent(newParent, false);

            screen.Hide(false);
        }
    }
}