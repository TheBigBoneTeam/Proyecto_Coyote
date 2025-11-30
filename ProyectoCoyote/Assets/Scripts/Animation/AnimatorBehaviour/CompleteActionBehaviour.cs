using UnityEngine;
/// <summary>
/// Accion que hace un solo loop pero no vuelve hasta terminar el 100 de la animacion
/// </summary>
public class CompleteActionBehaviour : ActionBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 1 && !finished)
        {
            finished = true;

            if (lastAttackInAction)
            {
                enemyAI.endCurrentAction(actionValue);
            }

        }
    }
}