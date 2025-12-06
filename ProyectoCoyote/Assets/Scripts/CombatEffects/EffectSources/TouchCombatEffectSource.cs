using CombatEffect;
using Services;
using Unity.VisualScripting;
using UnityEngine;

using CombatEffect;
namespace CombatEffect
{
    public class TouchCombatEffectSource : ATouchCombatEffectSource
    {
      protected  bool onlyToPlayer;
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
public interface IHealthSpawner:IService
{
    public void spawnOrb(Vector3 pos,int health);
    public void returnOrb(HealOrb orb);
}
