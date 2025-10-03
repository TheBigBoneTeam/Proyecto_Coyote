using Unity.VisualScripting;
using UnityEngine;

namespace CombatEffect
{
    [System.Serializable]
    public abstract class ACombatEffect
    {
        protected AGameCharacter objCharacter;
        public ACombatEffectSource source { get; private set; }
        public AGameCharacter owner { get; private set; }

        public bool Instant;
        public float Duration;
        protected float _currentDuration;
        public abstract void Activate(AGameCharacter character);
        public virtual bool Update()
        {
            _currentDuration -= Time.deltaTime;
            if (_currentDuration <= 0)
            {
                return true;
            }
            return false;
        }

        public abstract void End();
        public AGameCharacter getCharacter() => objCharacter;

        public ACombatEffect(ACombatEffectSource source)
        {
            this.source = source;
            owner = getOwner();
        }
        public AGameCharacter getOwner()
        {
            //if(source.GetType() != typeof(AOwnerableEffectSource))
            //{
            //    return null;
            //}
            //return source.GetComponent<AOwnerableEffectSource>().owner;
            return owner;
        }
    }
}
