using System;

namespace CombatEffect
{
    [System.Serializable]
    public class DamageEffect : ACombatEffect
    {
    public int _damage;

        public DamageEffect(ACombatEffectSource source,int damage):base(source)
        {
            Instant = true;
            this._damage = damage;
        }
        public override void Activate(AGameCharacter character)
        {
            this.objCharacter = character;
            character.getHit(_damage);
        }

        public override void End()
        {
            throw new System.NotImplementedException();
        }
    }
}
