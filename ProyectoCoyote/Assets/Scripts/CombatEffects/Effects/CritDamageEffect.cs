using System;

namespace CombatEffect
{
    [System.Serializable]
    public class CritDamageEffect : ACombatEffect
    {
    public int _critdamage;

        public CritDamageEffect(ACombatEffectSource source,int damage):base(source)
        {
            this._critdamage = damage;
        }
        public override void Activate(AGameCharacter character)
        {
            this.objCharacter = character;
            if (character)
            {
                character.getHit(_critdamage, true);
            }
        }

        public override void End()
        {

        }
        public CritDamageEffect()
        {

        }
    }
}
