using System;
using UnityEngine;

public class HookableObject : MonoBehaviour
{
    public bool canBeHooked;
    [SerializeField] bool dodge;
  [SerializeField]  CanvasGroup canvasGroup;
    EnemyAI enemyAI;
    Enemy enemy;
    EnemyAssetBehaviourRunner enemyAssetBehaviourRunner;
    [SerializeField]
    public bool Dodge
    {
        get => dodge; set { setHookDodge(value); }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }
        enemyAI = gameObject.GetComponent<EnemyAI>();
        enemy = gameObject.GetComponent<Enemy>();
        enemyAssetBehaviourRunner = gameObject.GetComponent<EnemyAssetBehaviourRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setHookDodge(bool dodge)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = dodge ? 1 : 0;
        }
        this.dodge = dodge;
    }

    internal void restart()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }
    }

    public void getHook()
    {
        print("getHookByPlayer");
        enemy.PlayAnimationCut("GetHook");
        enemyAssetBehaviourRunner.enabled = false;
        enemyAI.getHit();
    }
    public void endHook()
    {
        enemyAssetBehaviourRunner.enabled = true;
        if (GetComponent<BossEnemy>() != null)
        {
            GetComponent<BossEnemy>().NextFase();
        }

    }
    public void dodgeHook()
    {
        enemy.DodgeAttack(HitDirections.Outside);
    }
}
