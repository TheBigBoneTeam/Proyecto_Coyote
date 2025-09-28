public abstract class TimedEffect : ACombatEffect
{
    public TimedEffect(float duration)
    {
        Instant = false;
        this.Duration = duration;
        _currentDuration = duration;
    }
}
