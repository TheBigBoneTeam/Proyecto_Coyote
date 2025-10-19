using CombatEffect;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver:MonoBehaviour
{
    AGameCharacter character;
 [SerializeField]  List<HitDirections> directions;
 [SerializeField]   bool dodging;
    UnityEvent<ReceiverState> receiverStateEvent;
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
    private void Awake()
    {
        receiverStateEvent = new UnityEvent<ReceiverState>();

    }
    private void Start()
    {
        character = GetComponent<AGameCharacter>();
    }
    public void setDirection(HitDirections direction)
    {
        directions.Clear();
        directions.Add(direction);
        sendEvent();
    }
    public void addDirection(HitDirections direction)
    {
        directions.Add(direction);
        sendEvent();

    }
    public void setDodge(bool dodge)
    {
        dodging = dodge;
        sendEvent();
    }
    void sendEvent()
    {
        receiverStateEvent.Invoke(new ReceiverState(directions.ToArray(),dodging));

    }
    public void subscribeToStateChange(UnityAction<ReceiverState> response)
    {
        receiverStateEvent.AddListener(response);
        response(new ReceiverState(directions.ToArray(), dodging));

    }

    public void unSubscribeToStateChange(UnityAction<ReceiverState> response)
    {
        receiverStateEvent.AddListener(response);
    }

   

    public struct ReceiverState
    {
     public   HitDirections[] directions;
     public   bool isDodge;

        public ReceiverState(HitDirections[] directions, bool isDodge)
        {
            this.directions = directions;
            this.isDodge = isDodge;
        }
    }
}
