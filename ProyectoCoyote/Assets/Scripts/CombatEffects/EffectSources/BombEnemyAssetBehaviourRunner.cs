using BehaviourAPI.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
public class BombEnemyAssetBehaviourRunner : EnemyAssetBehaviourRunner
{
    public PushPerception ChosenAsAmmo { get; private set; }
    BullEnemyAssetBehaviourRunner _currentHeavy;
    
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
            if (currentHeavy != null)
            {
                if (_currentHeavy.GetComponent<Enemy>() != null)
                {
                    _currentHeavy.GetComponent<Enemy>().subscribeToDie(HeavyDie);
                    ChosenAsAmmo.Fire();
                }
            }
        }
    }

    private void HeavyDie(AGameCharacter arg0)
    {
        throw new NotImplementedException();
    }

  [field:SerializeField]  public bool charging { get; private set; }
    protected override void ModifyGraphs(Dictionary<string, BehaviourGraph> graphMap, Dictionary<string, PushPerception> pushPerceptionMap)
    {
        base.ModifyGraphs(graphMap, pushPerceptionMap);
        ChosenAsAmmo = pushPerceptionMap["ChosenAsAmmo"];
    }
    public void startCharging()
    {
        charging = true;
    }
    public override void restart()
    {
        base.restart();
        charging = false;
        currentHeavy = null;
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
}
