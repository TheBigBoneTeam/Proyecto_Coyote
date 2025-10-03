using UnityEngine;

namespace CombatEffect
{
    [System.Serializable]
    public abstract class ATimedEffect : ACombatEffect
    {
        public  abstract float getDuration();

        public ATimedEffect(ACombatEffectSource source,float duration):base(source)
        {
            
            _currentDuration = duration;
        }
        public ATimedEffect()
        {
            _currentDuration = getDuration();
        }
        public virtual bool Update()
        {
            _currentDuration -= Time.deltaTime;
            if (_currentDuration <= 0)
            {
                return true;
            }
            return false;
        }
        public override void Activate(AGameCharacter character)
        {
            _currentDuration = getDuration();
        }
        public override bool Instant()
        {
            return false;
        }
    }
}