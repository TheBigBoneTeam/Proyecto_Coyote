using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using Unity.VisualScripting.FullSerializer;
using BehaviourAPI.Core.Perceptions;


// Use this attribute to include this perception in a group: 
// [SelectionGroup("groupName")]
public class SeePlayer : UnityPerception
{   
    // Override this method to get references to unity components before the execution
    // using the "context" property.
    protected override void OnSetContext()
    {
        // To get a component of the object that is running this perception:
        // context.GameObject.GetComponent<SpriteRenderer>();

        // Some components like transform, rigidbody, etc. are directly accessible from context:
        // context.Transform
        // context.RigidBody
    }

    // Called at the start of the execution. Use it to initialize the perception.
    public override void Initialize()
    {

    }

    // Called every execution frame.
    public override bool Check()
    {
       return false;
    }

    // Called at the end of the execution. Use it to reset the perception.
    public override void Reset()
    {

    }
}
