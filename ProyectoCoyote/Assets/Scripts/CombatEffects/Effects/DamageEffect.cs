namespace CombatEffect
{
    public class DamageEffect : ACombatEffect
    {
        private int _damage;


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
