using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;
[SelectionGroup("MOVEMENT")]
public class ChasePlayerAction : ChaseAction
{
    public ChasePlayerAction(float speed, float maxDistance, float maxTime) : base(GameObject.FindAnyObjectByType<PlayerMovement>().transform,speed,maxDistance,maxTime)
    {
        this.speed = speed;
        this.maxTime = maxTime;
        this.maxDistance = maxDistance;


    }
    public ChasePlayerAction() {

    }
    public override void Start()
    {
        this.target = GameObject.FindAnyObjectByType<PlayerMovement>().transform;
        base.Start();
    }
}

public class shitAction : UnityAction
{
    public override Status Update()
    {
        throw new System.NotImplementedException();
    }
}