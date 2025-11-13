using CombatEffect;
using NUnit.Framework;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver:MonoBehaviour
{
    AGameCharacter character;
 [SerializeField] protected List<HitDirections> directions;
 [SerializeField] protected  bool dodging;
    [SerializeField] bool parrying;

    UnityEvent<ReceiverState> receiverStateEvent;
    IPerfectDodgeManager perfectDodgeManager;
    EnemyAI enemyAI;

   public void checkEffectSource(Attack attack)
    {
        print(gameObject.name + " checkEffectSource");
        if (enemyAI != null && !enemyAI.isLocked() && attack.owner.GetComponent<Player>() && !attack.GetComponent<baseBullet>()) {
            return;
        }
        if (!dodging || !canBeDodged(attack))
        {
            print("addeffedcts");
            attack.addEffectsToChar(character);
            //aOwnerableEffectSource.addEffectsToObj(character);
        }
        else
        {
            Debug.Log("Dodge");
            character.DodgeAttack(directions[0]);
            if (parrying && attack.Parreable)
            {
                Debug.Log("PARRY");
                if (!perfectDodgeManager.isSlowDown())
                {
                    perfectDodgeManager.StartSlowdown();
                }
            }
        }
    }

   protected virtual bool canBeDodged(Attack attack)
    {
        print("regularcanbedodged");

        return checkListIntersect(attack.HitDirectionsList, directions);
    }
    protected bool checkListIntersect(List<HitDirections> hitDirections, List<HitDirections> directions)
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
    protected virtual void Start()
    {
        perfectDodgeManager = ServiceLocator.Instance.Get<IPerfectDodgeManager>();
        character = GetComponent<AGameCharacter>();
        enemyAI = GetComponent<EnemyAI>();
    }
    public void setDirection(HitDirections direction)
    {
        directions.Clear();
        directions.Add(direction);
        sendDodgeEvent();
    }
    public void addDirection(HitDirections direction)
    {
        directions.Add(direction);
        sendDodgeEvent();

    }
    public void setDodge(bool dodge)
    {
        dodging = dodge;
        sendDodgeEvent();
    }
    public void setParry(bool parry)
    {
        parrying = parry;
        sendDodgeEvent();
    }
    void sendDodgeEvent()
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
