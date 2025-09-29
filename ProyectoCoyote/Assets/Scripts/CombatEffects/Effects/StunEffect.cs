using UnityEngine;

public class StunEffect: TimedEffect
{
    public StunEffect(float duration): base(duration)
    {
    }
    public override void Activate(AGameCharacter character)
    {
        Debug.Log($"StartStun with duration of {Duration}");
        this.character = character;
        character.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }

    public override void End()
    {
        Debug.Log("EndStun");

        character.gameObject.GetComponent<Renderer>().material.color = Color.gray;

    }
}
