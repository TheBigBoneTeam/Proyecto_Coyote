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

public class Player : AGameCharacter
{
    public override void Die()
    {
        print("PERDISTE");
    }
    public override bool isOtherTeam(AGameCharacter character)
    {
        return character.GetComponent<Enemy>() != null;
    }
}