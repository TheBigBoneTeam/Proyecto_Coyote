using CombatEffect;
using System.Linq;
using UnityEngine;

public class DamageReceiver:MonoBehaviour
{
    AGameCharacter character;
 [SerializeField]   HitDirections direction;
 [SerializeField]   bool dodging;

   public void checkEffectSource(Attack Attack)
    {
        if (!dodging || !Attack.HitDirections.Contains(direction))
        {
            Attack.addEffectsToChar(character);
            //aOwnerableEffectSource.addEffectsToObj(character);
        }
        else
        {
            Debug.Log("Dodge");
            character.DodgeAttack();
            if (Attack.Parreable)
            {
                Debug.Log("PARRY");
                Attack.owner.checkEffect(new StunEffect(2));
            }
        }
    }
    private void Start()
    {
        character = GetComponent<AGameCharacter>();
    }
    public void setDirection(HitDirections direction)
    {
        this.direction = direction;
    }
    public void setDodge(bool dodge)
    {
        dodging = dodge;
    }
 
}
