using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public bool endAction;
    Attack attackObj;
    AGameCharacter character;
    public bool isLocked;
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

    public enum BasicAttacks
    {
        AttackA,
        AttackB,
        AttackC,
        StanceA,
        StanceB,
    }
    public void LoadBasicAction(EnemyAI.BasicAttacks attack)
    {
        endAction = false;
        character.PlayAnimation(attack.ToString());

    }
    private void Start()
    {
        attackObj = GetComponentInChildren<Attack>();
        character = GetComponent<AGameCharacter>();
        endAction = false;
    }
}
