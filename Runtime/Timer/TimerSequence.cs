using System.Collections.Generic;
using DesertImage.Pools;

namespace DesertImage.Timers
{
    public class TimerSequence : Timer, ITimerSequence
    {
        private readonly Queue<TimerEntry> _queue = new Queue<TimerEntry>();

        private readonly Pool<ITimer> _pool;

        private TimerEntry _currentEntry;

        public TimerSequence(int id, Pool<ITimer> pool) : base(id)
        {
            _pool = pool;
        }

        public ITimerSequence Play(TimerEntry[] entries, float delay = 1, bool ignoreTimeScale = false)
        {
            _queue.Clear();

            foreach (var timerEntry in entries)
            {
                _queue.Enqueue(timerEntry);
            }

            PlayNext();

            return this;
        }

        public override void Tick()
        {
        }

        private void PlayNext()
        {
            if (_queue.Count == 0)
            {
                OnFinish?.Invoke(this);

                return;
            }

            _currentEntry = _queue.Dequeue();

            var timer = _pool.GetInstance();

            timer.OnFinish += OnTimerFinish;

            timer.Play(_currentEntry.Action, _currentEntry.Delay);
        }

        private void OnTimerFinish(ITimer timer)
        {
            timer.OnFinish -= OnTimerFinish;

            PlayNext();
        }
    }
}