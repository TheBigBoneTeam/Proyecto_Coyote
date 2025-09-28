using UnityEngine;

public class fakeStunEffect : StunEffect
{
    public fakeStunEffect(StunEffect effect) :base (effect.Duration)
    {
        this.Duration = effect.Duration;
    }
    public override void Activate(AGameCharacter character)
    {
        Debug.Log("StartFakeStun");
        this.character = character;
        character.gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    public override void End()
    {
        Debug.Log("EndFakeStun");
        character.gameObject.GetComponent<Renderer>().material.color = Color.white;

    }
}
