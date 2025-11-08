using NUnit.Framework;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class combatAreaManager : MonoBehaviour
{
    [Header("Enemigos y Acciones")]
    [SerializeField] Enemy[] startEnemies;
  [SerializeField]  StoryAction beforeCombatStoryAction;
    [SerializeField] StoryAction afterCombatStoryAction;

    [SerializeField] WaveData[] extraEnemyWaves;
    [Header("Colliders")]

    [SerializeField] GameObject areaColliders;
    [SerializeField] Collider triggerCollider;

    [Header("Coverturas")]

    [SerializeField] Cover[] allCover;

    [Header("Punto de Spawn")]

    [SerializeField] Transform respawnPoint;

     List<Enemy> deadEnemies;

    //Los enemigos 
    List<WaveData> functionalWaveDataList;

    int currentWave;
    Player _player;


    [SerializeField] bool started;

    EnemyLockOn lockOn;

    IGameStateManager gameStateManager;
    private void OnTriggerEnter(Collider other)
    {
        print("trigger"+other.gameObject.name);
        if (other.GetComponent<Player>() != null)
        {
            if (!started)
            {
                restart();
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();
    }
    void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        deadEnemies = new List<Enemy>();
        
        functionalWaveDataList = new List<WaveData>();
        functionalWaveDataList.Add(new WaveData(startEnemies,null));
        functionalWaveDataList.AddRange(extraEnemyWaves);
        lockOn = FindAnyObjectByType<EnemyLockOn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            print(getCoverSpot(out Vector3 pos) == null);

        }
    }

    public void enemyDie(AGameCharacter deadChar)
    {
      Enemy enemy =deadChar.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.unSubscribeToDie(enemyDie);
            if (!deadEnemies.Contains(enemy))
            {
                lockOn.resetWhenDie(enemy.transform);
                deadEnemies.Add(enemy);
            }
            if (deadEnemies.Count >= startEnemies.Length)
            {
                waveFinished();
            }
        }
    }
    public void startArea()
    {
        FindAnyObjectByType<Player>().setSpawnPoint(respawnPoint.position);
        gameStateManager.subscribeToRestart(restart);
        currentWave = 0;
        started = true;
        areaColliders.SetActive(true);
        if (beforeCombatStoryAction != null)
        {
            beforeCombatStoryAction.Execute(() =>
            {
                AudioManager.Instance.ChangeMusicAt(0, "OST Cañon - Pelea", 2f, 2f);
                gameStateManager.startCombat(); startWave();
            });

        }
        else
        {
            AudioManager.Instance.ChangeMusicAt(0, "OST Cañon - Pelea", 2f, 2f);
            gameStateManager.startCombat();
            startWave();
        }
    }
    void startWave()
    {
        if (functionalWaveDataList[currentWave].beforeWavestoryAction != null)
        {
            functionalWaveDataList[currentWave].beforeWavestoryAction.Execute(() => {
                gameStateManager.startCombat();
                foreach (var enemy in functionalWaveDataList[currentWave].enemies)
                {
                    enemy.activateEnemy();
                    enemy.subscribeToDie(enemyDie);
                }
            });
        }
        else
        {
            foreach (var enemy in functionalWaveDataList[currentWave].enemies)
            {
                enemy.activateEnemy();
                enemy.subscribeToDie(enemyDie);
            }
        }
      
    }
    public void restart()
    {
        currentWave = 0;
        int i = 0;
        foreach (var wave in functionalWaveDataList)
        {
            foreach(var enemy in wave.enemies)
            {
                enemy.restart();
            }
            i++;
        }
        startArea();
    }
    private void areaFinished()
    {
        if(afterCombatStoryAction != null)
        {
            gameStateManager.startCombat();
            afterCombatStoryAction.Execute(() =>
            {
                areaColliders.SetActive(false);
            });
        }
        else
        {
            areaColliders.SetActive(false);

        }

        // FindAnyObjectByType<winScreen>().Win();

    }
    private void waveFinished()
    {
        deadEnemies.Clear();
        currentWave++;
        if(currentWave == functionalWaveDataList.Count)
        {
            areaFinished();
        }
        else
        {
            startWave();
        }
    }

    public Cover getCoverSpot(out Vector3 hidePosition)
    {
        Transform objPos;
        Cover[] orderedCovers = allCover.OrderBy((c) => -((c.transform.position - _player.transform.position).sqrMagnitude)).ToArray();
        foreach (var cover in orderedCovers)
        {
            print(cover.name);
            if (cover.getBestPoint(_player.transform, out objPos) >= 0)
            {

                hidePosition = objPos.position;
                return cover;
                

            }
        }
        hidePosition = Vector3.zero;
        return null;
    }
}

[System.Serializable]
 class WaveData
{
   public Enemy[] enemies;
    public StoryAction beforeWavestoryAction;
   public WaveData(Enemy[] enemies, StoryAction storyAction )
    {
        Array.Copy(enemies, 0, enemies, 0, enemies.Length);
        this.enemies = enemies;
        this.beforeWavestoryAction = storyAction;
    }
}
