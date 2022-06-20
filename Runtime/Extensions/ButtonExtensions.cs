using System;
using UnityEngine.UI;

namespace Extensions
{
    public static class ButtonExtensions
    {
        public static void SetOnClick(this Button button, Action action)
        {
            if (!button) return;
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => action?.Invoke());
        }
    }
}