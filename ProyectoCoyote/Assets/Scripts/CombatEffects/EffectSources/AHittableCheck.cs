public abstract class AHittableCheck
{
   protected AGameCharacter owner;
    public abstract bool isHittable(AGameCharacter obj);
    public AHittableCheck(AGameCharacter owner)
    {
        this.owner = owner;
    }
}
public class AllCharacterHittable : AHittableCheck
{
    public AllCharacterHittable(AGameCharacter owner) : base(owner) { }

    public override bool isHittable(AGameCharacter obj)
    {
        return true;
    }
}
public class AllCharacterNoMeHittable : AHittableCheck
{
    public AllCharacterNoMeHittable(AGameCharacter owner) : base(owner) { }

    public override bool isHittable(AGameCharacter obj)
    {
        return owner != obj;
    }
}
public class OnlyOtherTeamHittable : AHittableCheck
{
    public OnlyOtherTeamHittable(AGameCharacter owner) : base(owner) { }
    public override bool isHittable(AGameCharacter obj)
    {
        return owner.isOtherTeam(obj);
    }
}
public enum HittableTypes
{
    allCharacters,
    allCharactersNoMe,
    onlyOtherTeam,
}
