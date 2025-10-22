using System;

namespace CombatEffect
{
    [System.Serializable]
    public class CritDamageEffect : ACombatEffect
    {
    public int _damage;

        public CritDamageEffect(ACombatEffectSource source,int damage):base(source)
        {
            this._damage = damage;
        }
        public override void Activate(AGameCharacter character)
        {
            this.objCharacter = character;
            character.getHit(_damage,true);
        }

        public override void End()
        {

        }
        public CritDamageEffect()
        {

        }
    }
}
