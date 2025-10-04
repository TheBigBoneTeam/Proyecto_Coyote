using Unity.VisualScripting;
using UnityEngine;

namespace CombatEffect
{
    [System.Serializable]
    public abstract class ACombatEffect
    {
        protected AGameCharacter objCharacter;
        public ACombatEffectSource source { get; private set; }

        public virtual bool Instant()
        {
            return true;
        }
        protected float _currentDuration;
        public abstract void Activate(AGameCharacter character);
      

        public abstract void End();
        public AGameCharacter getCharacter() => objCharacter;

        public ACombatEffect(ACombatEffectSource source)
        {
            this.source = source;
        }
        public ACombatEffect()
        {
            
        }
        public void setSource(ACombatEffectSource source)
        {
            this.source = source;
        }
    }
}
