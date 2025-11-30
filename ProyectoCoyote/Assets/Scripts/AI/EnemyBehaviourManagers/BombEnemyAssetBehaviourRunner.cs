using BehaviourAPI.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BombEnemyAssetBehaviourRunner : EnemyAssetBehaviourRunner
{
    public PushPerception ChosenAsAmmo { get; private set; }
    public PushPerception flyPerception { get; private set; }

    [SerializeField] BullEnemyAssetBehaviourRunner _currentHeavy;


    UnityEvent<BombEnemyAssetBehaviourRunner, bool> chargeAction;
    
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
        chargeAction?.Invoke(this,true);
    }
    public override void restart()
    {
        base.restart();
        chargeAction?.Invoke(this, false);
        chargeAction = new UnityEvent<BombEnemyAssetBehaviourRunner, bool>();
        charging = false;
        if (currentHeavy != null)
        {
            ChosenAsAmmo.Fire();
        }
    }
    private void OnDisable()
    {
        if (charging)
        {
            chargeAction?.Invoke(this, false);
            if (_currentHeavy != null)
            {
                if (_currentHeavy.GetComponent<BombEnemyAssetBehaviourRunner>() != null)
                {
                    _currentHeavy.GetComponent<Enemy>().unSubscribeToDie(HeavyDie);
                }
                _currentHeavy = null;
            }
        }
    }
    public void hitByPlayer()
    {
        print("hitByPlayer");
        if (charging)
        {
            Vector3 pos = transform.position;
            Vector3 camPos = new Vector3(Camera.main.transform.position.x,transform.position.y,Camera.main.transform.position.z);
            Vector3 obj = (2 * pos) - camPos;
            Debug.DrawRay(transform.position, obj-pos,Color.red,10);
            GetComponent<baseBullet>().StartBulletMovement(player, transform.position, obj);
        }
    }

    public void Fly()
    {
        flyPerception?.Fire();
        chargeAction?.Invoke(this,false);
    }

    public void subscribeToCharge(UnityAction<BombEnemyAssetBehaviourRunner, bool> response)
    {
         chargeAction.AddListener(response);
    }
    public void unSubscribeToCharge(UnityAction<BombEnemyAssetBehaviourRunner, bool> response)
    {
        chargeAction.RemoveListener(response);
    }
}
