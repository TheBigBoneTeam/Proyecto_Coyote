using Services;
using System;

public class Player : AGameCharacter
{
    IPerfectDodgeManager PerfectDodgeManager;
  public  int storedDamage;
    public override void Die()
    {
        dieEvent.Invoke();
        print("PERDISTE");
    }
    public override bool isOtherTeam(AGameCharacter character)
    {
        return character.GetComponent<Enemy>() != null;
    }
    public override void getHit(int damage, bool crit = false)
    {
        base.getHit(damage);
       
        if (PerfectDodgeManager.isSlowDown())
        {
            PerfectDodgeManager.StopSlowdown();
        }
        if (HealthPoint > 0)
        {
            storedDamage = damage;
        }
    }
    public void onParry()
    {
        getHealed(storedDamage);
        storedDamage = 0;
    }
    protected override void Start()
    {
        base.Start();
        PerfectDodgeManager = ServiceLocator.Instance.Get<IPerfectDodgeManager>();
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToStateChange(StateChange);
    }

    private void StateChange(object sender, stateData e)
    {
        switch (e.currentState)
        {
            case GameState.SlowDown:
                onParry();
                break;
                default: break;

        }
    }
}