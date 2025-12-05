using Services;
using System;
using System.Collections;
using UnityEngine;

public class CactusSpawner : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float PlayerDist;
    [SerializeField] float originalSpawnTime;
    [SerializeField] float extraRandom;
    [SerializeField] float currentSpawnTime;
    ObjectPool<SpawnableCactus> _cactusPool;
    [SerializeField] SpawnableCactus cactusPrefab;
    [SerializeField] int initCactusPoolSize;
    Player player;
    Coroutine currentNumerator;
  [SerializeField]  bool on;
    IGameStateManager gamestate;
 [SerializeField] public bool On
    {
        get => on; set { setPause(value); }

    }

    void setPause(bool val)
    {
        if (val)
        {
            if (!on)
            {
                GameState state = gamestate.getState();
                if (state == GameState.Combat || state == GameState.NonCombat || state == GameState.SlowDown)
                {
                    currentNumerator = StartCoroutine(spawnTimer());
                }

            }
        }
        else
        {
            if (on)
            {
                StopCoroutine(currentNumerator);
                currentNumerator = null;
            }
        }
        on = val;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamestate = ServiceLocator.Instance.Get<IGameStateManager>();
        gamestate.subscribeToStateChange(stateChange);
        gamestate.subscribeToRestart(restart);
        player = FindAnyObjectByType<Player>();
        setSpawnTime(-1);

    }
    private void Awake()
    {
        _cactusPool = new(cactusPrefab, initCactusPoolSize, true);

    }
    private void restart()
    {
        StopCoroutine(currentNumerator);
        currentNumerator = null;
        setSpawnTime(-1);
    }

    private void stateChange(object sender, stateData e)
    {
        GameState state = e.currentState;
        if (state == GameState.Paused || state == GameState.DeathScreen || state == GameState.Cutscene)
        {
            if (On && currentNumerator != null)
            {
                StopCoroutine(currentNumerator);
                currentNumerator = null;
            }
        }
        else
        {
            if (On && currentNumerator== null)
            {
                currentNumerator = StartCoroutine(spawnTimer());
            }

        }
    }
    IEnumerator spawnTimer()
    {
        while (true)
        {
            SpawnCactus();
            yield return new WaitForSeconds(currentSpawnTime + UnityEngine.Random.Range(0, extraRandom));
        }
    }
    public void setSpawnTime(float time)
    {
        if(time == -1)
        {
            currentSpawnTime = originalSpawnTime;
        }
        else
        {
            currentSpawnTime = time;
        }
    }
    void SpawnCactus()
    {

        if (getRandomPosition(out Vector3 pos))
        {
            SpawnableCactus cactus = _cactusPool.Get();
            cactus.Active = true;
            cactus.transform.position = pos;
            cactus.startAttack(player);
        }
    }

    private bool getRandomPosition(out Vector3 pos)
    {
        pos = Vector3.zero;
        int emergencyTries = 0;
        do
        {
            Vector2 circle = UnityEngine.Random.insideUnitCircle;
            Vector3 Pos = new Vector3(circle.x, 0, circle.y);
            pos = player.transform.position + (Pos * PlayerDist);
            emergencyTries++;
        } while (Vector3.Distance(transform.position, pos) > radius && emergencyTries <= 99);
        if (emergencyTries > 99)
        {
            return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void destroyCactus(SpawnableCactus spawnableCactus)
    {
        _cactusPool.Return(spawnableCactus);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);

    }
}
