using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using CombatEffect;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour,IMutex
{

    public bool endAction,cancelled;
    public bool counterOn, reactionOn;
  [SerializeField]  private bool doingReactCounter;
    Attack attackObj;
   [SerializeField] public Reaction reactionObj;
    [SerializeField] Reaction counterObj;

    AGameCharacter character;
    public bool Locked;
    GameObject player;
    public float seeDistance;

    public float attackDistance;

    IEnemyManager enemyManager;

    EnemyAssetBehaviourRunner _enemyAssetBehaviourRunner;

    public bool currentActionIsIdle { get; private set; }
    #region Calculo Distancia Jugador

    #endregion
    //public AnimatorOverrideController animatorOverrideController;
    //  [NamedArrayAttribute()] //Codigo para mostrar Palabras especificas en la lista de Stats
    ////  public AttackData[] ActionList = new AttackData[System.Enum.GetNames(typeof(BasicAttacks)).Length];

    //  internal AttackData getAttackData(BasicAttacks attack)
    //  {
    //   return   ActionList[(int)attack];

    //  }

    public void endCurrentAction()
    {
        print("endcurrentacytioncancelled: " + cancelled);  
        if (!cancelled || doingReactCounter)
        {
            endAction = true;

        }
        cancelled = false;

    }

    #region checksForAI
    public bool isLocked()
    {
        //print("checkLock"); 
        return player.GetComponent<EnemyLockOn>().currentTarget == this.transform;
    }


    public bool seePlayer()
    {
        if (DistanceWithPlayer() <= seeDistance)
        {
            return true;
        }
        return false;
    }
  
    public float DistanceWithPlayer()
    {
        return Vector3.Distance(player.transform.position, this.transform.position);
    }
    public bool TryGetAttackPriority()
    {
        print("TryGetAttackPriority");
        return enemyManager.attackingEnemy().getPermission(this,isLocked());
    }
    public bool ReturnAttackPriority()
    {
        if (!cancelled)
        {
            return enemyManager.attackingEnemy().returnPermission(this);

        }
        else
        {
           return false;
        }

    }
    public bool notDuringReactCounter()
    {
        print(!doingReactCounter);
     return   !doingReactCounter;
    }
    public void endReactionCounter()
    {
        counterOn = false;
        reactionOn = false;
        enemyManager.attackingEnemy().returnPermission(this);
        doingReactCounter = false;
        endAction = false;
    }
    public bool onAttackDistance()
    {
        float dist = DistanceWithPlayer();
        print($"{dist} : {attackDistance}");
        if (dist <= attackDistance)
        {
            return true;
        }
        return false;
    }
    #endregion
    public enum BasicActions
    {
        AttackA,
        AttackB,
        AttackC,
        StanceA,
        StanceB,
        StanceC,
        IdleAction,
        Walk,
        OutsideAttack,
        Idle,
        CombatIdle
    }
    public void LoadBasicAction(EnemyAI.BasicActions action, bool idle = false)
    {
        currentActionIsIdle = idle;
        if (!idle)
        {
            endAction = false;
        }
        character.PlayAnimation(action.ToString(),idle);

    }
    public void LoadAction(string action, bool idle = false)
    {
        currentActionIsIdle = idle;
        if (!idle)
        {
            endAction = false;
        }
        character.PlayAnimation(action, idle);

    }
    private void Start()
    {
        _enemyAssetBehaviourRunner = GetComponent<EnemyAssetBehaviourRunner>();
        FindAnyObjectByType<Player>().GetComponentInChildren<Attack>().subscribeToStateChange(PlayerAttackEvent);
        GetComponent<Enemy>().subscribeToDodgeAttack(PlayerHitDefenseEvent);
        currentActionIsIdle = false;
        attackObj = GetComponentInChildren<Attack>();
        character = GetComponent<AGameCharacter>();
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
        enemyManager = ServiceLocator.Instance.Get<IEnemyManager>();
        endAction = false;
    }

  

    private void PlayerAttackEvent(Attack.AttackState arg0)
    {
        if (reactionOn && isLocked())
        {
            _enemyAssetBehaviourRunner.FirePlayerAttack();
        }
    }
    private void PlayerHitDefenseEvent(HitDirections arg0)
    {
        if (counterOn && isLocked())
        {
            _enemyAssetBehaviourRunner.FirePlayerHitDefense();
        }
    }
    private void Update()
    {
        
    }

    public void startReaction()
    {
        currentActionIsIdle = false;
        cancelled = true;
        endAction = false;
        reactionOn = false;
        doingReactCounter = true;

        reactionObj.startReaction();
    }
    public void startCounter()
    {
        currentActionIsIdle = false;
        counterOn = false;
        cancelled = true;
        endAction = false;
        doingReactCounter = true;
        counterObj.startReaction();
    }
    
    public bool isCounterOn() => counterOn;
    public bool isReactionOn() => reactionOn;
    public void setCounter(bool counter)
    {
        counterOn = counter;
    }
    public void setReaction(bool reaction)
    {
        reactionOn = reaction;
    }

    #region Gizmos
    private void OnDrawGizmos()
    {
        //// Set the color with custom alpha.
        //Gizmos.color = new Color(1f, 0f, 0f, 1f); // Red with custom alpha

        //// Draw the sphere.
        //Gizmos.DrawSphere(transform.position, seeDistance);

        // Draw wire sphere outline.
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }

    public void endActionNode()
    {

        endAction = false;
    }

    public void givePriority()
    {
        _enemyAssetBehaviourRunner.endQueue();
    }
    #endregion
}
