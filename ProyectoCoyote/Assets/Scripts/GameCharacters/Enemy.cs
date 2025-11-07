using System.Diagnostics;

public class Enemy : AGameCharacter
{
    public override void Die()
    {
        dieEvent?.Invoke();
        Destroy(gameObject);
    }
    public override bool isOtherTeam(AGameCharacter character)
    {
        print(character.name);
        print(character.GetComponent<Enemy>() == null);
        return character.GetComponent<Enemy>() == null;
    }
    public override void getHit(int damage, bool crit = false)
    {
        base.getHit(damage, crit);
    }
}
