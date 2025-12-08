public class TutorialHookableObject : HookableObject
{
    public override void endHookGo()
    {
        gameObject.SetActive(false);
    }
}