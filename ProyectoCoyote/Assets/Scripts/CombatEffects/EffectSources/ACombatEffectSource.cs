using UnityEngine;

public abstract class ACombatEffectSource:MonoBehaviour
{
    private void Awake()
    {
        
    }
    protected abstract void addEffectTo(AGameCharacter character);


}
