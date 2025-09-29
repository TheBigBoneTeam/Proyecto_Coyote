using UnityEngine;

public abstract class ACombatEffect
{
    protected AGameCharacter character;

    public bool Instant;
    public float Duration;
    protected float _currentDuration;
    public abstract void Activate(AGameCharacter character);
    public virtual bool Update()
    {
        _currentDuration -= Time.deltaTime;
        if(_currentDuration<= 0)
        {
            return true;
        }
        return false;
    }

    public abstract void End();
}
