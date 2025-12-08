using Services;
using System;
using System.Collections.Generic;

public class PlayerDamageReceiver: DamageReceiver
{
    EnemyLockOn lockOn;

    protected override void Start()
    {
        base.Start();
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToStateChange(stateChange);
        lockOn = GetComponent<EnemyLockOn>();
    }

    private void stateChange(object sender, stateData e)
    {
        if (e.currentState == GameState.SlowDown)
        {
            Invincible = true;
        }
        else
        {
            Invincible = false;

        }
    }

    protected override bool canBeDodged(Attack attack)
    {
        print("playercanbedodged");

        
        if(attack.owner.transform == lockOn.currentTarget)
        {
            print("islocked");
            return checkListIntersect(attack.HitDirectionsList, directions);
        }
        else
        {
            print("isNotLocked");

            return checkListIntersect(new List<HitDirections> {HitDirections.Outside }, directions);
        }

    }
}
