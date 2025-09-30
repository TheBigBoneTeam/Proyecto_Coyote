namespace CombatEffect
{
    public abstract class TimedEffect : ACombatEffect
    {
        public TimedEffect(ACombatEffectSource source,float duration):base(source)
        {
            Instant = false;
            this.Duration = duration;
            _currentDuration = duration;
        }
    }
}