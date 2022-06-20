using DesertImage.Timers;

namespace DesertImage.Pools
{
    public class TimersSequencePool : Pool<ITimer>
    {
        private int _timersCount;

        private readonly Pool<ITimer> _pool;

        public TimersSequencePool(Pool<ITimer> pool)
        {
            _pool = pool;
        }

        protected override ITimer CreateInstance()
        {
            var timer = new TimerSequence(_timersCount, _pool);

            _timersCount++;

            return timer;
        }
    }
}