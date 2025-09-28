using UnityEngine;

public abstract class TouchCombatEffectSource : ACombatEffectSource
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AGameCharacter>())
        {
            addEffectTo(other.GetComponent<AGameCharacter>());
        }
    }

}
