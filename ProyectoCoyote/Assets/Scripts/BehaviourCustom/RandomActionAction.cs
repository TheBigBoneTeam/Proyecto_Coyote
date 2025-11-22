using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;

public class RandomActionAction : UnityAction
{
    public string BaseAction;
    public string FirstLetter;
    public string LastLetter;
    public bool idle;
    EnemyAI enemyAI;

    public override Status Update()
    {
        if (idle)
        {
            enemyAI.endActionNode();

            return Status.Success;
        }
        if (!idle && enemyAI.endAction)
        {
            enemyAI.endActionNode();
            return Status.Success;
        }
        return Status.Running;
    }
    public override void Start()
    {
        char firstletter = FirstLetter[0];
        char lastletter = LastLetter[0];
        int nums = lastletter - firstletter;
        char letter = (char)('A' + UnityEngine.Random.Range(0, nums));

        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        if (enemyAI.endAction)
        {
            Debug.Log("cagada");
        }
        enemyAI.LoadAction(BaseAction +letter, idle);
    }

}
