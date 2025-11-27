using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEditor;
using UnityEngine;

public class RandomActionAction : UnityAction
{
    public string BaseAction;
    public string FirstLetter;
    public string LastLetter;
    public bool idle;
    public int loops = 1;
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
        int min = firstletter - 'A';
        char letter = (char)('A' + UnityEngine.Random.Range(min, nums + 1));

        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        if (enemyAI.endAction)
        {
            Debug.Log("cagada");
        }
        bool isBlock = false;
        if (BaseAction.Contains("Stance"))
        {
            isBlock = true;
            if (enemyAI.blockingAction)
            {
                enemyAI.modifyActionLoop(loops);
                return;
            }
        }
        enemyAI.LoadAction(BaseAction +letter, idle,loops, isBlock);
    }

}