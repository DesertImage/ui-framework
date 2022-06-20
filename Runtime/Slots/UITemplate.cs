using UnityEngine;

namespace DesertImage.UI
{
    public interface IUITemplate
    {
        IScreenSlot[] Slots { get; }
    }

    public class UITemplate : MonoBehaviour, IUITemplate
    {
        public IScreenSlot[] Slots => slots;

        [SerializeField] private ScreenSlotBase[] slots;
    }
}