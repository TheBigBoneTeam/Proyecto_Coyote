using NUnit.Framework;
using System;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ActionBehaviour : StateMachineBehaviour
{
    public string DebugName;
    bool isIdle;
    protected int actionValue;
    public bool lastAttackInAction = true;
   protected bool finished;
   int loops;
    bool canEnd;
    // public bool lastAnimInAction = true;
  protected  EnemyAI enemyAI;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyAI = animator.gameObject.GetComponentInParent<EnemyAI>();
        //   Debug.Log("setOnAction " + DebugName + true);
        finished = false;
        enemyAI.setOnAction(true);
        enemyAI.setReaction(false);
        loops = enemyAI.currentActionLoops;
        canEnd = loops != -1;
        actionValue = enemyAI.currentAction;
        isIdle = enemyAI.currentActionIsIdle;
        // Debug.Log("StartAction"+isIdle + DebugName);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyAI.currentActionLoops == -1)
        {
            return;
        }
        if (stateInfo.normalizedTime > enemyAI.currentActionTime && !finished)
        {
            finished = true;
            if (!isIdle)
            {
                //  Debug.Log("EndAction"+DebugName);
                if (lastAttackInAction)
                {
                    enemyAI.endCurrentAction(actionValue);
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{


    //}
    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

public class EnemyDodgeBehaviour : ActionBehaviour
{

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.gameObject.GetComponentInParent<DamageReceiver>().setDodge(false);

    }
}
