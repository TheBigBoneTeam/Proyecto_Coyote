using UnityEngine;
using UnityEngine.Animations;

public class PlayerDodgeBehaviour : StateMachineBehaviour
{
    PlayerMovement move;
    bool finished = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        finished = false;
        move = animator.gameObject.GetComponentInParent<PlayerMovement>();
        if (move != null)
        {
            move.setCanMove(false);
            move.setCanAttack(false);
            move.setCanDodge(false);
        }
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.5f && !finished)
        {
            finished = true; if (move != null)
            {
                move.setCanMove(true);
                move.setCanAttack(true);
                move.setCanDodge(true);
            }
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponentInParent<DamageReceiver>().setDodge(false);
        if (!finished)
        {
            if (move != null)
            {
                move.setCanMove(true);
                move.setCanAttack(true);
                move.setCanDodge(true);
            }
        }
    }
}
