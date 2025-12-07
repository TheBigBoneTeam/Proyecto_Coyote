public class TutorialHookableObject : HookableObject
{
    public override void endHook()
    {
gameObject.SetActive(false);
    }
}