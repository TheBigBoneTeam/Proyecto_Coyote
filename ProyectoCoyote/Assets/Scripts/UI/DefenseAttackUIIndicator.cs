using NUnit.Framework;
using Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class DefenseAttackUIIndicator : MonoBehaviour
{
  [SerializeField] protected DamageReceiver DamageReceiver;
    [SerializeField] Attack attack;


    public GameObject[] attackUISignalers;
    public GameObject[] dodgeUISignalers;
    public GameObject middleDanger;
    public GameObject directionIndicator;
    public GameObject directionIndicatorFather;

    public Animator middleDangerAnimator;

    Dictionary<AGameCharacter, Attack> currentAttacksDictionary;
    List<baseBullet>currentBullets;
    List<BombEnemyAssetBehaviourRunner> currentExplosions;
 
    [SerializeField] Vector3 paddingPosition;

    protected CanvasGroup CanvasGroup;

    Player player;
  [SerializeField]  EnemyLockOn lockOn;

    public float minScale = 1f;
    public float maxScale = 4f;
    public float maxDistance = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentBullets = new List<baseBullet>();
        CanvasGroup = GetComponentInChildren<CanvasGroup>();
        currentAttacksDictionary = new Dictionary<AGameCharacter,Attack>();
        currentExplosions = new List<BombEnemyAssetBehaviourRunner>();
        if(middleDanger)
        middleDanger.SetActive(false);
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeCombatAreaChange(CombatAreaChange);
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToRestart(restart);

        player = GetComponentInParent<Player>();
        lockOn = GetComponentInParent<EnemyLockOn>();
        setUp();
        if(DamageReceiver != null)
        {
            player.subscribeToDodgeAttack(DodgeAttack);
        }
       // FindAnyObjectByType<PlayerMovement>().GetComponent<DamageReceiver>().subscribeToStateChange(StateChange);
    }

    private void DodgeAttack(HitDirections arg0)
    {
        if(arg0 == HitDirections.Outside)
        {
            if (middleDanger)
                middleDangerAnimator.Play("Blocked");
        }
    }

    private void CombatAreaChange(combatAreaManager manager, WaveData data)
    {
        print("combatAreaChange");
       
        restart();
        if (middleDanger)
            middleDanger.SetActive(false);
        foreach (var enemy in data.enemies)
        {
            BombEnemyAssetBehaviourRunner bombRunner = enemy.GetComponent<BombEnemyAssetBehaviourRunner>();
            if (bombRunner != null)
            {
                bombRunner.subscribeToCharge(chargeExplosion);
            }
            else
            {
                print("subcribe");
                enemy.attack.subscribeToStateChange(AttackHappeneed);
                enemy.subscribeToDie(enemyDie);
                Gun gun = enemy.GetComponent<Gun>();
                if (gun)
                {
                    gun.subscribeToShoot(shootGun);
                }
            }
        }
        if (data.spawner != null)
        {
            data.spawner.subscribeToCactusAttackChange(AttackHappeneed);
        }
    }

    protected void shootGun(baseBullet bullet)
    {
        bullet.subscribeToDestroy(bulletDestroy);
        print(bullet.owner);
        currentBullets.Add(bullet);
        AttackStateChange();
    }
    protected void bulletDestroy(baseBullet bullet)
    {
        currentBullets.Remove(bullet);
        AttackStateChange();
        //if(currentAttacksDictionary.TryGetValue(bullet.owner, out Attack attack))
        //{
        //    if (attack.GetComponent<baseBullet>().Equals(bullet))
        //    {
        //        currentAttacksDictionary.Remove(bullet.owner);
        //    }
        //}
    }
    protected void enemyDie(AGameCharacter enemy)
    {
        currentAttacksDictionary.Remove(enemy);
        enemy.attack.unSubscribeToStateChange(AttackHappeneed);
        enemy.unSubscribeToDie(enemyDie);
        AttackStateChange();
    }

    protected void setUp()
    {
        if (DamageReceiver != null)
        {
            DamageReceiver.subscribeToStateChange(DodgeStateChange);
        }
        if (attack != null)
        {
            attack.subscribeToStateChange(AttackHappeneed);
        }
        else
        {
            setEnable(false);
        }
    }
    // Update is called once per frame
 protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("Current Attack Dictionary Start:");
            foreach (KeyValuePair<AGameCharacter, Attack> value in currentAttacksDictionary)
            {
                print($"{value.Key} {value.Value.HitDirectionsList}");
            }
            print("Current Attack Dictionary End.");

        }
        if (currentBullets.Count > 0)
        {
            if (Time.frameCount % 5 == 0)
            {
                float angle = getAngle(currentBullets[0].transform.position);
                print(angle);
                directionIndicatorFather.transform.localEulerAngles = new Vector3(0, 0, -angle);
                float dist = Vector3.Distance(currentBullets[0].transform.position, transform.position);

                // Normalizar la distancia (0 = cerca, 1 = lejos)
                float t = Mathf.Clamp01(dist / maxDistance);

                // Interpolación del tamaño entre min y max
                float scale = Mathf.Lerp(maxScale, minScale, t);

                directionIndicator.transform.localScale = new Vector3(scale, scale, scale);
                //  directionIndicator.transform.localScale 
            }
        }
    }
    public void DodgeStateChange(DamageReceiver.ReceiverState state)
    {
        print("dodge state change");
        if (state.isDodge)
        {
            for (int i = 0; i < dodgeUISignalers.Length; i++)
            {
                setDodgeObject(dodgeUISignalers[i], state.directions.Contains((HitDirections)i));
            }
        }
        else
        {
            for (int i = 0; i < dodgeUISignalers.Length; i++)
            {
                setDodgeObject(dodgeUISignalers[i], false);
            }
            if (player != null)
            {
                setDodgeObject(dodgeUISignalers[2], true);
            }

        }
    }
    public void AttackHappeneed(Attack.AttackState state)
    {
        print("Attack Happened");
        if(state.Owner == null)
        {
            print("NULL OWNER");
            return;
        }
        if (state.attack == null)
        {
            print("Attack is Null");
            currentAttacksDictionary.Remove(state.Owner);
        }
        else
        {
            print("Attack is not Null");
            
            if (currentAttacksDictionary.ContainsKey(state.Owner))
            {
                currentAttacksDictionary[state.Owner] = state.attack;
            }
            else
            {
                currentAttacksDictionary.TryAdd(state.Owner, state.attack);
            }
        }
        AttackStateChange();
    }
    public void AttackStateChange()
    {
        print("Attack state change");
        bool anyLocked = false;
        AGameCharacter locked = lockOn.currentTarget?.GetComponent<AGameCharacter>();
        bool anyOutsideAttack = false;
        bool anyDistanceAttack = false;
        foreach (KeyValuePair<AGameCharacter,Attack> value in currentAttacksDictionary)
        {
            if (value.Key.Equals(locked))
            {
                anyLocked = true;
                for (int i = 0; attackUISignalers.Length > i; i++)
                {
                    setAttackObject(attackUISignalers[i], value.Value.HitDirectionsList.Contains((HitDirections)i));
                }
            }
            else
            {
                anyOutsideAttack = true;
            }
        }
        foreach(baseBullet bullet in currentBullets)
        {
            anyOutsideAttack = true;
            anyDistanceAttack = true;
            //Cambiar algo dependiendo de la cercania y tal
        }
        foreach (BombEnemyAssetBehaviourRunner bomb in currentExplosions)
        {
            anyOutsideAttack = true;
            //Cambiar algo dependiendo de la cercania y tal
        }
        if (middleDanger)
        {
            middleDanger.SetActive(anyOutsideAttack);
            directionIndicator.SetActive(anyDistanceAttack);
            if (anyOutsideAttack)
            {

                middleDangerAnimator.Play("Danger");

            }
            if (anyDistanceAttack)
            {
                float angle = getAngle(currentBullets[0].transform.position);
                print(angle);
                directionIndicatorFather.transform.localEulerAngles = new Vector3(0, 0, -angle);
                float dist = Vector3.Distance(currentBullets[0].transform.position, transform.position);

                // Normalizar la distancia (0 = cerca, 1 = lejos)
                float t = Mathf.Clamp01(dist / maxDistance);

                // Interpolación del tamaño entre min y max
                float scale = Mathf.Lerp(maxScale, minScale, t);

                directionIndicator.transform.localScale = new Vector3(scale, scale, scale);
            }
           
        }

        if (!anyLocked)
        {
            print("Attack state change: NoLocked");
            for (int i = 0; i < attackUISignalers.Length; i++)
            {
                setAttackObject(attackUISignalers[i], true);
            }
        }
        else
        {
            print("Attack state change: isLocked");
        }
    }
    public void restart()
    {
        currentAttacksDictionary.Clear();
        currentBullets.Clear();
        currentExplosions.Clear();
        AttackStateChange();
    }
    public void OutsideAttackChange(Attack.AttackState state)
    {
       // AttackHappeneed(state);
    }
    private void OnDestroy()
    {
        if(DamageReceiver != null)
       DamageReceiver.unSubscribeToStateChange(DodgeStateChange);

    }
    public void setEnable(bool enable)
    {
        CanvasGroup.alpha = enable ? 1:0;
    }

    public virtual void setCharacter(AGameCharacter character)
    {
        print("setCharacter");
        if(DamageReceiver != null)
        DamageReceiver.unSubscribeToStateChange(DodgeStateChange);
        if (character != null)
        {
            DamageReceiver = character.GetComponent<DamageReceiver>();
            if (DamageReceiver != null)
            {

                setEnable(true);
                this.transform.parent = character.transform;
                this.transform.localPosition = Vector3.zero + paddingPosition;
                DamageReceiver.subscribeToStateChange(DodgeStateChange);
            }
        }
        else
        {
            setEnable(false);
        }

    }
    public float getAngle(Vector3 pos)
    {
        Vector3 toTarget = pos - transform.position;

        return Vector3.SignedAngle(Camera.main.transform.forward, toTarget, Camera.main.transform.up);
    }
    public void setEnemy(AGameCharacter character)
    {
        //if(attack != null) 
        //attack.unSubscribeToStateChange(AttackHappeneed);
        if (character != null)
        {
            setEnable(true);
        }
        else
        {
            setEnable(false);
        }
        AttackStateChange();
    }
    public void unSetEnemy(AGameCharacter previousEnemy)
    {
        if(attack != previousEnemy.GetComponentInChildren<Attack>())
        {
            return;
        }
        if (attack != null)
        {
            attack.unSubscribeToStateChange(AttackHappeneed);
            attack = null;
        }
    }
    public void setDodgeObject(GameObject obj, bool on)
    {
        obj.GetComponent<Image>().enabled = on;
        obj.GetComponent<Image>().color = Color.white;
    }
    public void setAttackObject(GameObject obj, bool on)
    {
        obj.GetComponent<Image>().color = new Color(on ? 1 : 1, on ? 1 : 0, on ? 1 : 0, 1);

    }
   
    internal void chargeExplosion(BombEnemyAssetBehaviourRunner enemy,bool isCharging)
    {
        if (isCharging)
        {
            if (!currentExplosions.Contains(enemy))
            {
                currentExplosions.Add(enemy);
            }
        }
        else
        {
            currentExplosions.Remove(enemy);
        }
        AttackStateChange();
    }

}
