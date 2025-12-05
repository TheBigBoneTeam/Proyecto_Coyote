using System;
using UnityEngine;

public class AttacksAnimationEvent : MonoBehaviour
{
    Attack attack;
    EnemyAI enemyAI;
     DamageReceiver damageReceiver;
    Reaction Reaction;
    Reaction Counter;
    Gun gun;
    [SerializeField] private int actionValue;


    public void changeAttackDirections(HitDirections[] direction)
    {
        attack.setHitDirections(direction);
    }
    public void addAttackDirection(HitDirections direction)
    {
        attack.addHitDirection(direction);
    }
    public void setAttackDirection(HitDirections direction)
    {
        attack.setHitDirection(direction);
    }
    public void setAttackData(AttackData data)
    {
        attack.LoadData(data);
    }
    public void setParry(int parry)
    {
        if (parry == 0)
        {
            damageReceiver.setParry(false);
        }
        else
        {
            damageReceiver.setParry(true);
        }
    }
    public void setDodge(int dodge)
    {
        if (dodge == 0)
        {
            damageReceiver.setDodge(false/*,actionValue*/);
        }
        else
        {
            damageReceiver.setDodge(true/*, actionValue*/);
        }
    }
    public void setCounter(int dodge)
    {
        if (dodge == 0)
        {
            enemyAI.setCounter(false);
        }
        else
        {
            enemyAI.setCounter(true);
        }
    }
    public void setReaction(int dodge)
    {
        if (dodge == 0)
        {
            enemyAI.setReaction(false);
        }
        else
        {
            enemyAI.setReaction(true);
        }
    }
    public void setInvincible(int invincible)
    {
        if (invincible == 0)
        {
            damageReceiver.setInvincible(false);
        }
        else
        {
            damageReceiver.setInvincible(true);
        }
    }
    public void setSuperArmor(int armorOn)
    {
        if (armorOn == 1)
        {
            setDodgeDirection(HitDirections.Back);
            setDodge(1);
            setShaderBlockConfiguration(blockShaderConfigurations.None);
        }
        else
        {
           setDodge(0);
        }
    }
    public void setCounterAnim(string anim)
    {
        Counter.setAnim(anim);
    }
    public void setReactionAnim(string anim)
    {
        Reaction.setAnim(anim);
    }
    public void setCounterData(ReactionData data)
    {
        Counter.LoadData(data);
    }
    public void setReactionData(ReactionData data)
    {
        Reaction.LoadData(data);
    }
    public void setDodgeDirection(HitDirections direction)
    {
        damageReceiver.setDirection(direction);
    }
    public void addDodgeDirection(HitDirections direction)
    {
        damageReceiver.addDirection(direction);
    }
    public void setShaderBlockConfiguration(blockShaderConfigurations configuration)
    {
       damageReceiver.setBlockShaderConfiguration(configuration);
    }
    //public void endAttack()
    //{
    //    print("endAttack");
    //    enemyAI.endCurrentAction();
    //}
    public void ShootPlayer()
    {
        Vector3 pos = FindFirstObjectByType<Player>().transform.position;
        print(pos);
        gun.Shoot(pos);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attack = GetComponentInChildren<Attack>();
        enemyAI = GetComponentInParent<EnemyAI>();
        damageReceiver = GetComponentInParent<DamageReceiver>();
        gun = GetComponentInParent<Gun>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void setActionValue(int actionValue)
    {
        this.actionValue = actionValue;
    }
    public void Die()
    {
        print("eventDie"+name);
        GetComponentInParent<AGameCharacter>().Die();
    }
}
