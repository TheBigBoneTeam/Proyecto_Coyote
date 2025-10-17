using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public bool endAction;
    Attack attackObj;
    AGameCharacter character;
    public bool Locked;
    GameObject player;
    //public AnimatorOverrideController animatorOverrideController;
    //  [NamedArrayAttribute()] //Codigo para mostrar Palabras especificas en la lista de Stats
    ////  public AttackData[] ActionList = new AttackData[System.Enum.GetNames(typeof(BasicAttacks)).Length];

    //  internal AttackData getAttackData(BasicAttacks attack)
    //  {
    //   return   ActionList[(int)attack];

    //  }

    public void endCurrentAction()
    {
        print("endActionD");
        endAction = true;
    }
    public bool isLocked()
    {
        //print("checkLock"); 
        return player.GetComponent<EnemyLockOn>().currentTarget == this.transform;
    }
    //public bool seePlayer()
    //{
    //    if(Vector3.Distance())
    //}
    public enum BasicActions
    {
        AttackA,
        AttackB,
        AttackC,
        StanceA,
        StanceB,
        StanceC,
        Idle
    }
    public void LoadBasicAction(EnemyAI.BasicActions action, bool idle = false)
    {
        endAction = false;
        character.PlayAnimation(action.ToString(),idle);

    }
    private void Start()
    {
        attackObj = GetComponentInChildren<Attack>();
        character = GetComponent<AGameCharacter>();
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
        endAction = false;
    }
}
