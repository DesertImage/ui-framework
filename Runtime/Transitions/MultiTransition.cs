using System;
using UnityEngine;

namespace DesertImage.UI
{
    public class MultiTransition : ATransition
    {
        [SerializeField] private ATransition[] transitions;

        private int _activeTransitions;

        private Action _callback;

        public override void Init(Transform transf)
        {
            base.Init(transf);

            foreach (var transition in transitions)
            {
                transition.Init(transition.transform);
            }
        }

        public override void Play(Transform transform, Action callback = null)
        {
            Cancel();

            _activeTransitions = transitions.Length;

            _callback = callback;

            if (_activeTransitions == 0) callback?.Invoke();

            for (var i = 0; i < _activeTransitions; i++)
            {
                transitions[i].Play(transitions[i].transform, TransitionDone);
            }
        }

        public override void ShowHard(Transform transform, Action callback = null)
        {
            foreach (var transition in transitions)
            {
                transition.ShowHard(transform, callback);
            }
        }

        public override void Cancel()
        {
            foreach (var transition in transitions)
            {
                transition.Cancel();
            }
        }

        private void TransitionDone()
        {
            _activeTransitions--;

            if (_activeTransitions > 0) return;

            _callback?.Invoke();
        }
    }
}