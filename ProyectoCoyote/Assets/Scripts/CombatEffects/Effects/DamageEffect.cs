using System;

namespace CombatEffect
{
    [System.Serializable]
    public class DamageEffect : ACombatEffect
    {
    public int _damage;

        public DamageEffect(ACombatEffectSource source,int damage):base(source)
        {
            this._damage = damage;
        }
        public override void Activate(AGameCharacter character)
        {
            this.objCharacter = character;
            if (character)
            {
                character.getHit(_damage, source.GetComponent<Attack>().getMainDirection());
            }
        }

        public override void End()
        {
            throw new System.NotImplementedException();
        }
        public DamageEffect()
        {

        }
    }
}
