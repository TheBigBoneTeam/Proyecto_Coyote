using Services;
using System;
using UnityEngine;
public class BossEnemy : Enemy
{
    [SerializeField] int currentFase;

  [SerializeField]  BossFaseData[] bossFases;
    [SerializeField] BossEnemyAssetBehaviourRunner bossEnemyAssetBehaviourRunner;

    [SerializeField] int baseCactusAttackTime;

    [SerializeField] CactusSpawner spawner;
    [SerializeField] int healthDropDistance = 0;

    [SerializeField] bool learningCurrentFase;
    public bool NextFase()
    {
        currentFase++;
        if (currentFase >= bossFases.Length)
        {
            return false;
        }
        if (bossEnemyAssetBehaviourRunner.bossState == BossState.Melee)
        {
            ServiceLocator.Instance.Get<IHealthSpawner>().spawnOrb(transform.position, healthDrop, false);
        }
        else
        {
            ServiceLocator.Instance.Get<IHealthSpawner>().spawnOrb(transform.position, healthDropDistance, false);
        }
        startFase();
        return true;
    }
    public void finishHook()
    {
        if (bossEnemyAssetBehaviourRunner.bossState == BossState.Distance)
        {
            NextFase();
        }
    }
    public void startFase()
    {
        bossEnemyAssetBehaviourRunner.setBossState(bossFases[currentFase].state);
        if(bossFases[currentFase].life != -1)
        {
            setHealthPoint(bossFases[currentFase].life);
        }
        else
        {
            setHealthPoint(_maxHealthPoint);
        }
        print(currentFase);
        print(spawner == null);
        print(bossFases[currentFase]);
        learningCurrentFase = bossFases[currentFase].learningPhase;
        spawner.On = bossFases[currentFase].hasCactusAttack;
        if (bossFases[currentFase].cactusAttackTime == -1)
        {
            spawner.setSpawnTime(baseCactusAttackTime);
        }
        else
        {
            spawner.setSpawnTime(bossFases[currentFase].cactusAttackTime);
        }
    }
    public override void Die()
    {
        if (NextFase())
        {
            return;
        }
        base.Die();
    }
    public override void restart()
    {
        currentFase = 0;
        base.restart();
        startFase();

    }
    public bool learningPhase() => learningCurrentFase;
    public override void activateEnemy(bool active)
    {
        print($"activateEnemy{name} {active}");
        gameObject.SetActive(ActiveBeforeFight ? true : active);
        GetComponent<EnemyAssetBehaviourRunner>().enabled = active;
        bossEnemyAssetBehaviourRunner = GetComponent<BossEnemyAssetBehaviourRunner>();

        if (active)
        {
          //  AudioManager.Instance.PlayBossMusic();
            GetComponent<EnemyAssetBehaviourRunner>().restart();
        }
    }
    protected override void Start()
    {
        base.Start();
        bossEnemyAssetBehaviourRunner = GetComponent<BossEnemyAssetBehaviourRunner>();

    }
}
[System.Serializable]
class BossFaseData
{
    public BossState state;
    public bool hasCactusAttack;
    public int cactusAttackTime;
    public int life;
    public bool learningPhase = true;
    public BossFaseData(BossState state, bool hasCactusAttack, int life, int cactusAttackTime, bool learningPhase = true)
    {
        this.state = state;
        this.hasCactusAttack = hasCactusAttack;
        this.life = life;
        this.cactusAttackTime = cactusAttackTime;
        this.learningPhase = learningPhase;
    }
}