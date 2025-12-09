using System;

namespace CombatEffect
{
    [System.Serializable]
    public class DamageEffect : ACombatEffect
    {
        public int _damage;

        public DamageEffect(ACombatEffectSource source, int damage) : base(source)
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

        }
        public DamageEffect()
        {

        }
    }
    public class HealEffect : ACombatEffect
    {
        public int _heal;
        public HealEffect(ACombatEffectSource source, int _heal) : base(source)
        {
            this._heal = _heal;
        }
        public override void Activate(AGameCharacter character)
        {
            this.objCharacter = character;
            if (character)
            {
                character.getHealed(_heal);
            }
        }

        public override void End()
        {

        }

        public HealEffect()
        {

        }
    }
}
