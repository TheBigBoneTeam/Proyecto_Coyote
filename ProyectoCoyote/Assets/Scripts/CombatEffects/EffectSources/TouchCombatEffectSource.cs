using UnityEngine;

namespace CombatEffect
{
    public class TouchCombatEffectSource : ATouchCombatEffectSource
    {
        bool onlyToPlayer;
        protected override void OnTriggerEnter(Collider other)
        {
            AGameCharacter character = other.GetComponent<AGameCharacter>();
            if (character)
            {
                if (!onlyToPlayer || character.GetComponent<Player>())
                {
                    addEffectsToChar(character);
                }

            }
        }
    }
}
