namespace DesertImage.Timers
{
    public interface ITimerSequence : ITimer
    {
        ITimerSequence Play(TimerEntry[] entries, float delay = 1f, bool ignoreTimeScale = false);
    }
}