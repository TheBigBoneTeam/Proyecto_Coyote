using UnityEngine;

public class getHookedBehaviour : StateMachineBehaviour
{
    EnemyAssetBehaviourRunner enemyAssetRunner;
    EnemyAI enemyAI;
    bool finished;

    //// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    finished = false;
    //    enemyAssetRunner = animator.gameObject.GetComponentInParent<EnemyAssetBehaviourRunner>();
    //    if (enemyAssetRunner != null)
    //    {

    //        enemyAssetRunner.enabled = false;
    //        EnemyAI enemyAI = animator.gameObject.GetComponentInParent<EnemyAI>();
    //        enemyAI.getHit();
    //    }

    //    animator.gameObject.GetComponentInParent<DamageReceiver>().setDodge(false);
    //    animator.gameObject.GetComponentInParent<DamageReceiver>().setParry(false);
    //}

    //// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    ////override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    ////{
    ////    
    ////}
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Debug.Log($"{stateInfo.loop} {stateInfo.normalizedTime}");
    //    if (stateInfo.normalizedTime > 0.9 && !finished)
    //    {
    //        finished = true;
    //        if (enemyAssetRunner != null)
    //        {
    //            enemyAssetRunner.enabled = true;
    //        }
    //    }
    //}
}