using System;

namespace DesertImage.Timers
{
    public interface ITimer : ITick, IPoolable
    {
        public Action<ITimer> OnFinish { get; set; }

        int Id { get; }

        float Time { get; }


        public void Play(Action action, float timeDelay = 1f, bool ignoreTimeScale = false);

        public void Play(Action<Timer> action, float timeDelay = 1f, bool ignoreTimeScale = false);

        void Stop();

        void PlayAndReturnToPool();
    }
}