using CombatEffect;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageReceiver:MonoBehaviour
{
    AGameCharacter character;
 [SerializeField]  List<HitDirections> directions;
 [SerializeField]   bool dodging;

   public void checkEffectSource(Attack Attack)
    {
        if (!dodging || !checkListIntersect(Attack.HitDirections, directions))
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

    private bool checkListIntersect(List<HitDirections> hitDirections, List<HitDirections> directions)
    {
        foreach (HitDirections hitDirection in hitDirections)
        {
           if(directions.Contains(hitDirection))
            {
                return true;
            }

        }
        return false;
    }

    private void Start()
    {
        character = GetComponent<AGameCharacter>();
    }
    public void setDirection(HitDirections direction)
    {
        directions.Clear();
        directions.Add(direction);
    }
    public void addDirection(HitDirections direction)
    {
        directions.Add(direction);
    }
    public void setDodge(bool dodge)
    {
        dodging = dodge;
    }
 
}
