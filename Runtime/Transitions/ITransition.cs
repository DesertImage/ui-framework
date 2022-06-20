using System;
using UnityEngine;

namespace DesertImage.UI
{
    public interface ITransition
    {
        bool IsInProcess { get; }

        void Play(Transform transform, Action callback = null);

        void ShowHard(Transform transform, Action callback = null);

        void Cancel();
    }
}