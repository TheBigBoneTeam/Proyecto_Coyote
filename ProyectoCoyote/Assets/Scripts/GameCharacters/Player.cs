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