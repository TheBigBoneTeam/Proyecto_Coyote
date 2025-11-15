using NUnit.Framework;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class combatAreaManager : MonoBehaviour
{
    [Header("Enemigos y Acciones")]
    [SerializeField] Enemy[] startEnemies;
  [SerializeField]  StoryAction beforeCombatStoryAction;
    [SerializeField] StoryAction afterCombatStoryAction;
    [Header("Coverturas")]

    [SerializeField] Cover[] initCovers;
    [Header("Rocas")]

    [SerializeField] baseBullet[] initAmmo;
    [Header("Oleadas Extras")]

    [SerializeField] WaveData[] extraEnemyWaves;
    [Header("Colliders")]

    [SerializeField] GameObject areaColliders;
    [SerializeField] Collider triggerCollider;


    Cover[] currentCovers;
    List<baseBullet> currentAmmo;

    [Header("Punto de Spawn")]

    [SerializeField] Transform respawnPoint;

     List<Enemy> deadEnemies;

    //Los enemigos 
    List<WaveData> functionalWaveDataList;

    int currentWaveIndex;
    WaveData currentWaveData;
    Player _player;

    Action ammoChangeAction;


    [SerializeField] bool started;
    [SerializeField] bool finished;

    EnemyLockOn lockOn;

    IGameStateManager gameStateManager;

    
    private void OnTriggerEnter(Collider other)
    {
      

        if (other.GetComponent<Player>() != null)
        {
            print("trigger" + other.gameObject.name);
            print("triggerpn" + transform.parent.name);
            if (!started)
            {

                startArea();

            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();
        gameStateManager.subscribeToRestart(restart);
        gameStateManager.subscribeToStateChange(StateChange);
        setAreas();
        restart();

    }

    private void StateChange(object sender, stateData e)
    {
        //if(e.currentState == GameState.DeathScreen)
        //{
        //    foreach (var wave in functionalWaveDataList)
        //    {
        //        print(name);
        //        foreach (var enemy in wave.enemies)
        //        {
        //            enemy.activateEnemy(false);

        //        }
        //    }
        //}
    }

    private void setAreas()
    {
        foreach (var wave in functionalWaveDataList)
        {
            print(name);
            foreach (var enemy in wave.enemies)
            {
                enemy.setArea(this);
                
            }
        }
    }
    void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        deadEnemies = new List<Enemy>();
        
        functionalWaveDataList = new List<WaveData>();
        functionalWaveDataList.Add(new WaveData(startEnemies,null,true,null,initCovers,initAmmo, respawnPoint));
        functionalWaveDataList.AddRange(extraEnemyWaves);
        lockOn = FindAnyObjectByType<EnemyLockOn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && started && !finished)
        {
            ServiceLocator.Instance.Get<IEnemyManager>().DebugPositions();
        }
    }

    public void enemyDie(AGameCharacter deadChar)
    {
      Enemy enemy =deadChar.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.unSubscribeToDie(enemyDie);
            if (enemy.GetComponent<baseBullet>() != null || enemy.GetComponent<BullEnemyAssetBehaviourRunner>() != null)
            {
                ammoChangeAction?.Invoke();
                currentAmmo.Remove(enemy.GetComponent<baseBullet>());
            }
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

        started = true;
        areaColliders.SetActive(true);
        _player.setSpawnPoint(respawnPoint.position);
        gameStateManager.subscribeToRestart(restart);

        currentWaveIndex = 0;
        deadEnemies.Clear();
        foreach (Transform child in areaColliders.transform)
        {
            child.gameObject.SetActive(true);
        }
        if (beforeCombatStoryAction != null && !functionalWaveDataList[0].waveFinished)
        {
            beforeCombatStoryAction.Execute(() =>
            {
                //AudioManager.Instance.ChangeMusicAt(0, "OST Cañon - Pelea", 2f, 2f);
                startWave();
            });

        }
        else
        {
            //AudioManager.Instance.ChangeMusicAt(0, "OST Cañon - Pelea", 2f, 2f);
            startWave();
        }
    }
    void startWave()
    {
        
         currentWaveData = functionalWaveDataList[currentWaveIndex];
        if(currentWaveIndex>0 && currentWaveData.spawnPoint != null)
        {
            for (int i = currentWaveIndex - 1; i >= 0; i--)
            {
                if (functionalWaveDataList[i].waveFinished)
                {
                    break;
                }
                functionalWaveDataList[i].waveFinished = true;
            }
        }
        if (currentWaveData.waveFinished == false)
        {
            if (currentWaveData.Covers != null && currentWaveData.Covers.Length > 0)
            {
                currentCovers = functionalWaveDataList[currentWaveIndex].Covers;
            }
            if (currentWaveData.ammo != null && currentWaveData.ammo.Length > 0)
            {
                currentAmmo = functionalWaveDataList[currentWaveIndex].ammo.ToList();
            }
            if (currentWaveData.spawnPoint != null)
            {
                _player.setSpawnPoint(currentWaveData.spawnPoint.position);
            }
            if (currentWaveData.beforeWavestoryAction != null)
            {
                currentWaveData.beforeWavestoryAction.Execute(() =>
                {
                    finalStartWave();

                });
            }
            else
            {
                finalStartWave();
            }
        }else  {
            print("WavwCandelled");
            waveFinished();
        }

    }
    void finalStartWave()
    {
        foreach (var enemy in currentWaveData.enemies)
        {
            enemy.activateEnemy(true);
            enemy.subscribeToDie(enemyDie);
            baseBullet bullet = enemy.GetComponent<baseBullet>();
            if(bullet != null)
            {
                currentAmmo ??= new List<baseBullet>();
                currentAmmo.Add(bullet);
            }
        }
        ammoChangeAction?.Invoke();
        if (currentAmmo != null)
        {
            foreach (var ammo in currentAmmo)
            {
                ammo.subcribeToShoot((a) => { currentAmmo.Remove(a); ammoChangeAction?.Invoke(); print("removeammo newammocount: " + currentAmmo.Count); });
            }
        }
        if (currentWaveData.colliderTurnOffBefore)
        {
            currentWaveData.colliderTurnOffBefore.SetActive(true);
        }
        gameStateManager.startCombat(this, currentWaveData);

    }
    
    public WaveData getCurrentWaveData()=>currentWaveData;
    public void restart()
    {
        if (!finished)
        {
            started = false;
            currentWaveIndex = 0;
            int i = 0;
            areaColliders.SetActive(false);
            foreach (var wave in functionalWaveDataList)
            {
                foreach (var enemy in wave.enemies)
                {
                    enemy.unSubscribeToDie(enemyDie);
                    enemy.restart();
                    if (i > 0)
                    {
                        enemy.activateEnemy(false);
                    }
                }
                i++;
            }
            foreach (WaveCaller waveCaller in GetComponentsInChildren<WaveCaller>())
            {
                waveCaller.restart();
            }
        }
    }
    private void areaFinished()
    {
        currentWaveData = null;
        finished = true;
        if(afterCombatStoryAction != null)
        {
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
        currentWaveIndex++;
      
        if (currentWaveIndex == functionalWaveDataList.Count)
        {
            areaFinished();
        }
        else
        {
            if (functionalWaveDataList[currentWaveIndex].spawnPoint != null && functionalWaveDataList[currentWaveIndex].autoStart)
            {
                currentWaveData.waveFinished = true;
            }
            currentWaveData = functionalWaveDataList[currentWaveIndex];
            if (currentWaveData.colliderTurnOffBefore)
            {
                currentWaveData.colliderTurnOffBefore.SetActive(false);
            }
            if (currentWaveData.autoStart)
            {
                startWave();
            }
        }
    }

    public void startWaveExternal(int wave)
    {
        if (!started)
        {
            currentWaveIndex = wave;
            for (int i = currentWaveIndex - 1; i >= 0; i--)
            {
                functionalWaveDataList[i].waveFinished = true;
            }
            startArea();
            return;
        }
        if(currentWaveIndex == wave)
        {
            startWave();
        }
    }

    public Cover getCoverSpot(Enemy enemy,out Vector3 hidePosition,out int coverIndex)
    {
        Transform objPos;
         Cover[] orderedCovers = currentCovers.OrderBy((c) => -((c.transform.position - _player.transform.position).sqrMagnitude)).ToArray();
        //Cover[] orderedCovers = currentCovers.OrderBy<>
        foreach (var cover in orderedCovers)
        {
            print(cover.name);
            coverIndex = cover.getBestPoint(enemy,_player.transform, out objPos);
            print(coverIndex);
            if (coverIndex >= 0)
            {
                

                hidePosition = objPos.position;
                return cover;
                

            }
        }
        hidePosition = Vector3.zero;
        coverIndex = -1;
        return null;
    }
    public baseBullet[] getAllBullets()
    {
        List<baseBullet> bullets = new List<baseBullet>();
        foreach(baseBullet ammo in currentAmmo)
        {
            if(ammo == null) continue;
            bullets.Add(ammo);
        }
        //foreach(Enemy enemy in currentWaveData.enemies)
        //{
        //    baseBullet bulet = enemy.gameObject.GetComponent<baseBullet>();
        //   if(bulet == null) continue;
        //   bullets.Add(bulet);
        //}
        return bullets.ToArray();
    }

    public void subscribeToAmmoChange(Action response)
    {
        ammoChangeAction += response;
    }
    public void unSubscribeToAmmoChange(Action response)
    {
        ammoChangeAction += response;
    }
    public void changeInAmmoOwnership()
    {
        ammoChangeAction?.Invoke();
}
}

[System.Serializable]
public class WaveData
{

   public Enemy[] enemies;
    public bool autoStart;
    public StoryAction beforeWavestoryAction;
    public GameObject colliderTurnOffBefore;
    public Cover[] Covers;
    public baseBullet[] ammo;
    public Transform spawnPoint;
   public bool waveFinished;
   public WaveData(Enemy[] enemies, StoryAction storyAction ,bool autoStart,GameObject colliderTurnOffBefore, Cover[] covers, baseBullet[] ammo, Transform spawnpoint)
    {
        this.enemies = enemies;
        this.beforeWavestoryAction = storyAction;
        this.autoStart = autoStart;
        this.colliderTurnOffBefore = colliderTurnOffBefore;
        this.Covers = covers;
        this.ammo = ammo;
        this.spawnPoint = spawnpoint;
        waveFinished = false;
    }
}
