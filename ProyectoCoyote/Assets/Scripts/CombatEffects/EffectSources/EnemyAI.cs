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
    public float seeDistance;

    public float attackDistance;

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

        endAction = true;
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
        Idle,
        Walk
    }
    public void LoadBasicAction(EnemyAI.BasicActions action, bool idle = false)
    {
        if (!idle)
        {
            endAction = false;
        }
        character.PlayAnimation(action.ToString(),idle);

    }
    private void Start()
    {
        attackObj = GetComponentInChildren<Attack>();
        character = GetComponent<AGameCharacter>();
        player = FindAnyObjectByType<PlayerMovement>().gameObject;

        endAction = false;
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
    #endregion
}
