using System;
using UnityEngine;

namespace DesertImage.UI
{
    [ExecuteInEditMode]
    public abstract class ATransition : MonoBehaviour, ITransition
    {
        public bool IsInProcess { get; protected set; }

        public virtual void Init(Transform transf)
        {
        }

        public abstract void Play(Transform transform, Action callback = null);
        public abstract void ShowHard(Transform transform, Action callback = null);

        public abstract void Cancel();
    }
}