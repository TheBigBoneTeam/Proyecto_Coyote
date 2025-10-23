using NUnit.Framework;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ActionBehaviour : StateMachineBehaviour
{
    public string DebugName;
    bool isIdle;
    
    // public bool lastAnimInAction = true;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isIdle = animator.gameObject.GetComponentInParent<EnemyAI>().currentActionIsIdle;
        Debug.Log("StartAction"+isIdle + DebugName);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (!isIdle)
        {
            Debug.Log("EndAction"+DebugName);
            animator.gameObject.GetComponentInParent<EnemyAI>().endCurrentAction();
            animator.gameObject.GetComponentInChildren<Attack>().LoadData(null);
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
