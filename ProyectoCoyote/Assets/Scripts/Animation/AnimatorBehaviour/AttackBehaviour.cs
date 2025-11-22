using UnityEngine;

public class AttackBehaviour : ActionBehaviour
{
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if (!animator.gameObject.GetComponentInParent<EnemyAI>().currentActionIsIdle && lastAttackInAction)
        {
            animator.gameObject.GetComponentInParent<EnemyAI>().ReturnAttackPriority(actionValue);
        }
    }
}
