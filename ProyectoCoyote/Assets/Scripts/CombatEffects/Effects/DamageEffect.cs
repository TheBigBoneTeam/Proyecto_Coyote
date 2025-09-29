public class DamageEffect : ACombatEffect
{
    private int _damage;


    public  DamageEffect(int damage)
    {
        Instant = true;
        this._damage = damage;
    }
    public override void Activate(AGameCharacter character)
    {
        this.character = character;
        character.getHit(_damage);
    }

    public override void End()
    {
        throw new System.NotImplementedException();
    }
}
