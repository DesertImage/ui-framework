using System;
using DesertImage.ECS;
using DesertImage.Managers;
using Managers;

namespace DesertImage.Timers
{
    public class Timer : ITimer
    {
        public Action<ITimer> OnFinish { get; set; }

        public Timer(int id)
        {
            Id = id;
        }

        public int Id { get; }
        public float Time { get; private set; }

        protected float TargetTime;
        protected Action Action;

        protected bool IsPlaying;

        protected bool IsIgnoreTimescale;

        public virtual void Tick()
        {
            if (!IsPlaying) return;

            Time += IsIgnoreTimescale ? UnityEngine.Time.unscaledDeltaTime : UnityEngine.Time.deltaTime;

            if (Time < TargetTime) return;

            IsPlaying = false;

            Action.Invoke();

            Action = null;

            OnFinish?.Invoke(this);

            ReturnToPool();
        }

        #region PLAY / STOP / RESET

        public void Play(Action action, float timeDelay = 1f, bool ignoreTimeScale = false)
        {
            IsPlaying = true;

            Action = action;

            TargetTime = timeDelay;

            IsIgnoreTimescale = ignoreTimeScale;
        }

        public void Play(Action<Timer> action, float timeDelay = 1f, bool ignoreTimeScale = false)
        {
            IsPlaying = true;

            Action = () => action?.Invoke(this);

            TargetTime = timeDelay;

            IsIgnoreTimescale = ignoreTimeScale;
        }

        public void Stop()
        {
            IsPlaying = false;

            Reset();
        }

        public void PlayAndReturnToPool()
        {
            if (!IsPlaying) return;

            Action?.Invoke();

            ReturnToPool();
        }

        private void Reset()
        {
            Time = 0f;

            Action = null;

            TargetTime = 0.3f;
        }

        #endregion

        #region POOL STUFF

        public void OnCreate()
        {
            Reset();

            Core.Instance?.Get<TimersUpdater>()?.Add(this);
        }

        public void ReturnToPool()
        {
            Stop();

            Core.Instance?.Get<TimersUpdater>().Remove(this);

            Core.Instance?.Get<ManagerTimers>().ReturnInstance(this);
        }

        #endregion
    }
}