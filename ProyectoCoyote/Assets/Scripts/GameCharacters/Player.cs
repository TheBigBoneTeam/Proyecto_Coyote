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
    [SerializeField] int parryHeal = 1;
    [SerializeField] int _closeEnemies = 0;
    [SerializeField] float CloseEnemyCalculationIntervalThreshold = 2;
    [SerializeField] float _currentCloseEnemyCalculationInterval;

    [SerializeField] float _closeEnemyDistance;
    [SerializeField] LayerMask _enemyMask;


    public override void Die()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        _currentCloseEnemyCalculationInterval = _closeEnemies = 0;
        // gameObject.SetActive(false);
        dieEvent.Invoke(this);
        playerMovement.setCanAttack(false);
        playerMovement.setCanMove(false);
        playerMovement.setCanDodge(false);
        print("PERDISTE");
    }
    public override bool isOtherTeam(AGameCharacter character)
    {
        
        if (character.GetComponent<Enemy>() != null)
            return true;
        //if(character.GetComponent<SpawnableCactus>() != null) return true;
        return false;
    }
    public override void getHit(int damage,HitDirections directions, bool crit = false)
    {
        if (PerfectDodgeManager.isSlowDown())
        {
            return;
           // PerfectDodgeManager.StopSlowdown();
        }
        base.getHit(damage,directions);
       
      
        if (HealthPoint > 0)
        {
            storedDamage = damage;
            hook.ResetTarget(true);
        }
    }
    public void onParry()
    {
        getHealed(parryHeal);
        playerMovement.setCanAttack(true);
        playerMovement.setCanDodge(true);
            
        storedDamage = 0;
    }
    public override void restart()

    {
        gameObject.SetActive(true);
        base.restart();
        _currentCloseEnemyCalculationInterval= _closeEnemies = 0;
        GetComponent<Rigidbody>().isKinematic = false;
        playerMovement.setCanAttack(true);
        playerMovement.setCanMove(true);
        playerMovement.setCanDodge(true);

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
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeCombatAreaChange(combatAreaChange);
        startPos = transform.position;
        lockOn = GetComponent<EnemyLockOn>();
        hook = GetComponent<Gancho>();
    }

    private void combatAreaChange(combatAreaManager manager, WaveData data)
    {
       // setHealthPoint(_maxHealthPoint);
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
                playerMovement.setCanDodge(false);

                break;

        }
        if (e.oldState == GameState.Cutscene)
        {
            playerMovement.setCanMove(true);
            playerMovement.setCanAttack(true);
            playerMovement.setCanDodge(true);
        }
    }

    internal void setSpawnPoint(Vector3 respawnPoint)
    {
        print("setSpawnPoint");
        startPos = respawnPoint;
    }

    internal int GetCloseEnemies()
    {
        if (_currentCloseEnemyCalculationInterval <= 0)
        {
            CalculateCloseEnemies();
            _currentCloseEnemyCalculationInterval = CloseEnemyCalculationIntervalThreshold;
        }
        return _closeEnemies;
    }

    private void CalculateCloseEnemies()
    {
        print("CalculateCloseEnemies");
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, _closeEnemyDistance, _enemyMask);
        _closeEnemies = nearbyTargets.Length;

    }

    protected override void Update()
    {
        base.Update();
        _currentCloseEnemyCalculationInterval = Mathf.Max(0, _currentCloseEnemyCalculationInterval-=Time.deltaTime);
    }
}