using BehaviourAPI.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
public class BombEnemyAssetBehaviourRunner : EnemyAssetBehaviourRunner
{
    public PushPerception ChosenAsAmmo { get; private set; }
    public PushPerception flyPerception { get; private set; }

    [SerializeField] BullEnemyAssetBehaviourRunner _currentHeavy;
    
    public BullEnemyAssetBehaviourRunner currentHeavy
    {
        get
        {
            return _currentHeavy;
        }
        set
        {
            if (_currentHeavy != null)
            {

                _currentHeavy.GetComponent<Enemy>().unSubscribeToDie(HeavyDie);
                
            }
            _currentHeavy = value;
            print("_currentHeavyBomb" + value);

            if (currentHeavy != null)
            {

                if (_currentHeavy.GetComponent<Enemy>() != null)
                {
                    _currentHeavy.GetComponent<Enemy>().subscribeToDie(HeavyDie);
                    print("chosenAsAmmo");

                    ChosenAsAmmo.Fire();
                }
            }
        }
    }

    private void HeavyDie(AGameCharacter arg0)
    {
        BullEnemyAssetBehaviourRunner runner = arg0.gameObject.GetComponent<BullEnemyAssetBehaviourRunner>();
        if(runner && currentHeavy ==runner)
        {
            currentHeavy = null;
        }
    }

  [field:SerializeField]  public bool charging { get; private set; }
    protected override void ModifyGraphs(Dictionary<string, BehaviourGraph> graphMap, Dictionary<string, PushPerception> pushPerceptionMap)
    {
        base.ModifyGraphs(graphMap, pushPerceptionMap);
        ChosenAsAmmo = pushPerceptionMap["ChosenAsAmmo"];
        flyPerception = pushPerceptionMap["FlyPerception"];
    }
    public void startCharging()
    {
        charging = true;
    }
    public override void restart()
    {
        base.restart();
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
        charging = false;
        if (_currentHeavy != null)
        {
            if (_currentHeavy.GetComponent<BombEnemyAssetBehaviourRunner>() != null)
            {
                _currentHeavy.GetComponent<Enemy>().unSubscribeToDie(HeavyDie);
            }
            _currentHeavy = null;
        }
    }

    public void hitByPlayer()
    {
        print("hitByPlayer");
        if (charging)
        {
            Vector3 pos = transform.position;
            Vector3 obj = (2 * pos) - player.transform.position;
            GetComponent<baseBullet>().StartBulletMovement(player, transform.position, obj);
        }
    }

    public void Fly()
    {
        flyPerception?.Fire();
    }
}
