public class Player : AGameCharacter
{
    public override void Die()
    {
        dieEvent.Invoke();
        print("PERDISTE");
    }
    public override bool isOtherTeam(AGameCharacter character)
    {
        return character.GetComponent<Enemy>() != null;
    }
}