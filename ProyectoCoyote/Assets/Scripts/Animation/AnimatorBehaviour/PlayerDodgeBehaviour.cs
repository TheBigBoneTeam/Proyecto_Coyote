using UnityEngine;

public class PlayerDodgeBehaviour : StateMachineBehaviour
{
    PlayerMovement move;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        move = animator.gameObject.GetComponentInParent<PlayerMovement>();
        if (move != null)
        {
            move.setCanMove(false);
            move.setCanAttack(false);
            move.setCanDodge(false);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
            animator.gameObject.GetComponentInParent<DamageReceiver>().setDodge(false);
        if (move != null)
        {
            move.setCanMove(true);
            move.setCanAttack(true);
            move.setCanDodge(true);
        }
    }
}
