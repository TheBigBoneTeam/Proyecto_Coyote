using Services;
using System;
using UnityEngine;

public class Player : AGameCharacter
{
    IPerfectDodgeManager PerfectDodgeManager;
  public  int storedDamage;
    PlayerMovement playerMovement;
    EnemyLockOn lockOn;
    Gancho hook;

    public override void Die()
    {
        playerMovement.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        // gameObject.SetActive(false);
        dieEvent.Invoke(this);
        playerMovement.setCanAttack(false);
        playerMovement.setCanMove(false);
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
            hook.ResetTarget();
        }
    }
    public void onParry()
    {
        getHealed(storedDamage);
        storedDamage = 0;
    }
    public override void restart()

    {
        gameObject.SetActive(true);
        playerMovement.enabled = false;
        base.restart();
        playerMovement.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;

        playerMovement.setCanAttack(true);
        playerMovement.setCanMove(true);
        lockOn.ResetTarget();
        hook.ResetTarget();
    }
    protected override void Start()
    {

        base.Start();
        PerfectDodgeManager = ServiceLocator.Instance.Get<IPerfectDodgeManager>();
        playerMovement = GetComponent<PlayerMovement>();    
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToStateChange(StateChange);
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToRestart(restart);
        lockOn = GetComponent<EnemyLockOn>();
        hook = GetComponent<Gancho>();
    }

    private void StateChange(object sender, stateData e)
    {
        switch (e.currentState)
        {
            case GameState.SlowDown:
                onParry();
                break;
                default: break;
                case GameState.Cutscene:
                playerMovement.setCanMove(false);
                playerMovement.setCanAttack(false);

                break;

        }
        if (e.oldState == GameState.Cutscene)
        {
            playerMovement.setCanMove(true);
            playerMovement.setCanAttack(true);
        }
    }

    internal void setSpawnPoint(Vector3 respawnPoint)
    {
        print("setSpawnPoint");
        startPos = respawnPoint;
    }
}