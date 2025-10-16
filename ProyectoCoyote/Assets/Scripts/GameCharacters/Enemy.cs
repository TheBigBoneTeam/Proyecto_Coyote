using System.Diagnostics;

public class Enemy : AGameCharacter
{
    public override void Die()
    {
        Destroy(gameObject);
    }
    public override bool isOtherTeam(AGameCharacter character)
    {
        return character.GetComponent<Enemy>() == null;
    }
}
