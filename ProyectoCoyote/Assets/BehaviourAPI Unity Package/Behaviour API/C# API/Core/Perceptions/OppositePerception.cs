using BehaviourAPI.Core;
using BehaviourAPI.Core.Perceptions;
using System;
using UnityEngine;
[Serializable]
public class OppositePerception : Perception
{
    [SerializeReference] public Perception perception;
    public override bool Check()
    {
        UnityEngine.Debug.Log($"OppositePerception {perception == null} {!perception.Check()} {perception.ToString()} ");
        if (perception == null)
        {
            return false;
        }
        return !perception.Check();
    }
    public override void Initialize()
    {
        perception.Initialize();
    }

    /// <summary>
    /// <inheritdoc/>
    /// Reset all the sub perceptions.
    /// </summary>
    public override void Reset()
    {
        perception.Reset();
    }

    /// <summary>
    /// <inheritdoc/>
    /// Pauses all the subp perceptions.
    /// </summary>
    public override void Pause()
    {
        perception.Pause();
    }

    /// <summary>
    /// <inheritdoc/>
    /// Unpauses all the subp perceptions.
    /// </summary>
    public override void Unpause()
    {
        perception.Unpause();
    }

    ///// <summary>
    ///// Passes the execution context to the sub perceptions.
    ///// </summary>
    ///// <param name="context"><inheritdoc/></param>
    public override void SetExecutionContext(ExecutionContext context)
    {
        perception.SetExecutionContext(context);
    }

    /// <summary>
    /// <inheritdoc/>
    /// Copies the subperceptions one by one.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    public override object Clone()
    {
        OppositePerception newperception = (OppositePerception)base.Clone();
        newperception.perception = this.perception;
        return newperception;
    }
}
