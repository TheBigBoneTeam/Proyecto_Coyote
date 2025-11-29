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
    [SerializeField] protected bool Invincible;
    [SerializeField] bool parrying;

    UnityEvent<ReceiverState> receiverStateEvent;
    IPerfectDodgeManager perfectDodgeManager;
 [SerializeField]   EnemyAI enemyAI;
    List<string> currentBlockShaderParts;

   public void checkEffectSource(Attack attack)
    {
        if (enemyAI != null && !enemyAI.isLocked() && attack.owner.GetComponent<Player>() && !attack.GetComponent<baseBullet>()) {
            return;
        }
        if(Invincible){
            return;
        }
        if (!dodging || !canBeDodged(attack))
        {
            attack.addEffectsToChar(character);
            //aOwnerableEffectSource.addEffectsToObj(character);
        }
        else
        {
            character.DodgeAttack(directions[0]);
            if (parrying && attack.Parreable)
            {
                if (!perfectDodgeManager.isSlowDown())
                {
                    perfectDodgeManager.StartSlowdown();
                }
            }
        }
    }

   protected virtual bool canBeDodged(Attack attack)
    {
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
        currentBlockShaderParts = new List<string>();
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
        print($"SetDodge: {dodge}");
        if (enemyAI)
        {
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                changeBlockColor(skinnedMeshRenderer);
            }

        }

        sendDodgeEvent();
    }
    public void setDodgeFromEvent(bool dodge, int actionvalue)
    {
        if (enemyAI == null)//Es Jugador
        {
            setDodge(dodge);
            return;
        }
        if (actionvalue == enemyAI.currentAction)
        {
            setDodge(dodge);
        }
    }
    void changeBlockColor(SkinnedMeshRenderer skinMesh)
    {
        skinMesh.material.SetInt("_isBlockL", (dodging && currentBlockShaderParts.Contains("_isBlockL"))? 1 : 0);
        skinMesh.material.SetInt("_isBlockR", (dodging && currentBlockShaderParts.Contains("_isBlockR")) ? 1 : 0);
        skinMesh.material.SetInt("_isBlockWeapon", (dodging && currentBlockShaderParts.Contains("_isBlockWeapon")) ? 1 : 0);


    }
    public void setBlockShaderConfiguration(blockShaderConfigurations blockShaderConfigurations)
    {
        currentBlockShaderParts.Clear();
        switch (blockShaderConfigurations)
        {
            case blockShaderConfigurations.LeftArm:
                currentBlockShaderParts.Add("_isBlockL");
                break;
            case blockShaderConfigurations.RightArm:
                currentBlockShaderParts.Add("_isBlockR");

                break;
            case blockShaderConfigurations.BothArms:
                currentBlockShaderParts.Add("_isBlockL");
                currentBlockShaderParts.Add("_isBlockR");

                break;
            case blockShaderConfigurations.Weapon:
                currentBlockShaderParts.Add("_isBlockWeapon");

                break;
            case blockShaderConfigurations.All:
                currentBlockShaderParts.Add("_isBlockWeapon");
                currentBlockShaderParts.Add("_isBlockL");
                currentBlockShaderParts.Add("_isBlockR");

                break;
            case blockShaderConfigurations.None:
                break;
        }
    }
    public void setInvincible(bool invincible)
    {
        Invincible = invincible;
    }
    public void setParry(bool parry)
    {
        parrying = parry;
        sendDodgeEvent();
    }
    public void clearDirection()
    {
        directions.Clear();
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
public enum blockShaderConfigurations{
    LeftArm,RightArm,BothArms,Weapon,All,None
}
