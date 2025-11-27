using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEditor;
using UnityEngine;

public class PlayWaitAction : UnityAction
{
    [SerializeField] string attack;
    [SerializeField] bool random;
    [SerializeField] string FirstLetter;
    [SerializeField] string LastLetter;

    EnemyAI enemyAI;
    public override Status Update()
    {
        if (enemyAI.hasPriority)
        {
            enemyAI.endActionNode();

            return Status.Success;
        }
        return Status.Running;
    }
    public override void Start()
    {
        bool isBlock = false;
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        if (enemyAI.endAction)
        {
            Debug.Log("cagada");
        }
        if (attack.Contains("Stance"))
        {
            Debug.Log($"Contains Stance{enemyAI.blockingAction}");
            if (enemyAI.blockingAction)
            {
                enemyAI.modifyActionLoop(-1);
                return;
            }
            isBlock = true;
        }
        if (random)
        {
            char firstletter = FirstLetter[0];
            char lastletter = LastLetter[0];
            int nums = lastletter - firstletter;
            int min = firstletter - 'A';
            char letter = (char)('A' + UnityEngine.Random.Range(min, nums + 1));
            enemyAI.LoadAction(attack+letter,false,-1,isBlock);

        }
        else
        {
            enemyAI.LoadAction(attack,false,-1);
        }
    }
}
