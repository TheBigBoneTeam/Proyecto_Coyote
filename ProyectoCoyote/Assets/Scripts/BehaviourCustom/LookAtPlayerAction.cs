using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;

public class LookAtPlayerAction : UnityAction
{
    Transform target;

    public override void Start()
    {
        this.target = GameObject.FindAnyObjectByType<PlayerMovement>().transform;
    }
    public override Status Update()
    {
        Vector3 lookTarget = new Vector3(target.position.x, context.Transform.position.y, target.position.z);
        context.Transform.LookAt(lookTarget);
        return Status.Success;
    }
}