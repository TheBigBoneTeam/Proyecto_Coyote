using System.Collections.Generic;

public class PlayerDamageReceiver: DamageReceiver
{
    EnemyLockOn lockOn;

    protected override void Start()
    {
        base.Start();
        lockOn = GetComponent<EnemyLockOn>();
    }
    protected override bool canBeDodged(Attack attack)
    {
        print("playercanbedodged");
        
        if(attack.owner.transform == lockOn.currentTarget)
        {
            print("islocked");
            return checkListIntersect(attack.HitDirections, directions);
        }
        else
        {
            print("isNotLocked");

            return checkListIntersect(new List<HitDirections> {HitDirections.Outside }, directions);
        }

    }
}
