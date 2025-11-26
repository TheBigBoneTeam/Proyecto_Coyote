using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
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
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        if (enemyAI.endAction)
        {
            Debug.Log("cagada");
        }
        if (random)
        {
            char firstletter = FirstLetter[0];
            char lastletter = LastLetter[0];
            int nums = lastletter - firstletter;
            int min = firstletter - 'A';
            char letter = (char)('A' + UnityEngine.Random.Range(min, nums + 1));
            enemyAI.LoadAction(attack+letter);

        }
        else
        {
            enemyAI.LoadAction(attack);
        }
    }
}
