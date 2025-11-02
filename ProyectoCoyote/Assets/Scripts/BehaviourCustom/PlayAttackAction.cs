using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;

public class PlayAttackAction : UnityAction
{

    /// <summary>
    /// El Scriptable Object del ataque
    /// </summary>
    [SerializeField] string attack;

    public override Status Update()
    {
        return Status.Running;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public override void Start()
    {
        context.GameObject.GetComponent<EnemyAI>().LoadAction(attack);
    }


}
