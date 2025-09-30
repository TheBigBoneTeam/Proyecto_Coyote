using UnityEngine;

namespace CombatEffect
{
    public abstract class ATouchCombatEffectSource : ACombatEffectSource
    {
        protected virtual void OnTriggerEnter(Collider other)
        {
            AGameCharacter character = other.GetComponent<AGameCharacter>();
            if (character)
            {

                addEffect(createEffect(character));
                if (oneUse)
                {
                    destroySource();
                }
            }
        }

    }
}
