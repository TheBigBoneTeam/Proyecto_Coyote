using System.Linq.Expressions;
using UnityEngine;

public class getHitBehaviour : StateMachineBehaviour
{
    PlayerMovement move;
    EnemyAssetBehaviourRunner enemyAssetRunner;
    EnemyAI enemyAI;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)

    {
        move = animator.gameObject.GetComponentInParent<PlayerMovement>();
        if (move != null)
        {
            move.setCanMove(false);
            move.setCanAttack(false);
        }
        else
        {
            enemyAssetRunner = animator.gameObject.GetComponentInParent<EnemyAssetBehaviourRunner>(); 
            if (enemyAssetRunner != null)
            {

                enemyAssetRunner.enabled = false;

            }
        }
        animator.gameObject.GetComponentInParent<DamageReceiver>().setDodge(false);
        animator.gameObject.GetComponentInParent<DamageReceiver>().setParry(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (move != null)
        {
            move.setCanMove(true);
            move.setCanAttack(true);
        }
        if (enemyAssetRunner != null)
        {
            enemyAssetRunner.enabled = true;
            EnemyAI enemyAI = animator.gameObject.GetComponentInParent<EnemyAI>();
            enemyAI.ReturnAttackPriority(enemyAI.currentAction);
            enemyAI.setOnAction(false);

        }
    }

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
