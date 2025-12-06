using UnityEngine;

public class playerAttackehaviour : StateMachineBehaviour
{
    PlayerMovement move;
    bool finished = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        move = animator.gameObject.GetComponentInParent<PlayerMovement>();
        finished = false;
        if (move != null)
        {
            move.setCanMove(false);
            move.setCanAttack(false);
            move.setCanDodge(false);

        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.9f && !finished)
        {
            finished = true; if (move != null)
            {
                move.setCanMove(true);
                move.setCanAttack(true);
                move.setCanDodge(true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (move != null)
        //{
        //    move.setCanMove(true);
        //    move.setCanAttack(true);
        //    move.setCanDodge(true);

        //}
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
