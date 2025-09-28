public class Enemy : AGameCharacter
{
    public override void Die()
    {
        Destroy(gameObject);
    }
}