using System;
using UnityEngine;

public class AttacksAnimationEvent : MonoBehaviour
{
    Attack attack;
    EnemyAI enemyAI;
     DamageReceiver damageReceiver;
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
            attack.setParry(false);
        }
        else
        {
            attack.setParry(true);
        }
    }
    public void setDodge(int dodge)
    {
        if (dodge == 0)
        {
            damageReceiver.setDodge(false);
        }
        else
        {
            damageReceiver.setDodge(true);
        }
    }
    public void setDodgeDirection(HitDirections direction)
    {
        damageReceiver.setDirection(direction);
    }
    public void addDodgeDirection(HitDirections direction)
    {
        damageReceiver.addDirection(direction);
    }
    public void endAttack()
    {
        print("endAttack");
        enemyAI.endCurrentAction();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attack = GetComponentInChildren<Attack>();
        enemyAI = GetComponentInParent<EnemyAI>();
        damageReceiver = GetComponentInParent<DamageReceiver>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
