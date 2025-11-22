using UnityEngine;

public class PlayerDodgeBehaviour : StateMachineBehaviour
{

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
            animator.gameObject.GetComponentInParent<DamageReceiver>().setDodge(false);
        
    }
}
