using NUnit.Framework;
using Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class DefenseAttackUIIndicator : MonoBehaviour
{
  [SerializeField] protected DamageReceiver DamageReceiver;
    [SerializeField] Attack attack;


    public GameObject[] attackUISignalers;
    public GameObject[] dodgeUISignalers;
    public GameObject middleDanger;
    public Animator middleDangerAnimator;

    Dictionary<AGameCharacter, HitDirections[]> currentAttacksDictionary;

    [SerializeField] Vector3 paddingPosition;

    CanvasGroup CanvasGroup;

    Player player;
  [SerializeField]  EnemyLockOn lockOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CanvasGroup = GetComponentInChildren<CanvasGroup>();
        currentAttacksDictionary = new Dictionary<AGameCharacter, HitDirections[]>();
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeCombatAreaChange(CombatAreaChange);
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
            middleDangerAnimator.Play("Blocked");
        }
    }

    private void CombatAreaChange(combatAreaManager manager, WaveData data)
    {
        foreach(var enemy in data.enemies)
        {
            print("subcribe");
            enemy.attack.subscribeToStateChange(AttackHappeneed);
            enemy.subscribeToDie(enemyDie);
        }
    }

    private void enemyDie(AGameCharacter enemy)
    {
        enemy.attack.unSubscribeToStateChange(AttackHappeneed);
        enemy.unSubscribeToDie(enemyDie);
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
    void Update()
    {
        
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
        }
    }
    public void AttackHappeneed(Attack.AttackState state)
    {
        print("Attack Happened");
        if (state.hitDirections == null)
        {
            print("Attack is Null");
            currentAttacksDictionary.Remove(state.Owner);
        }
        else
        {
            print("Attack is not Null");

            if (currentAttacksDictionary.ContainsKey(state.Owner))
            {
                currentAttacksDictionary[state.Owner] = state.hitDirections;
            }
            else
            {
                currentAttacksDictionary.TryAdd(state.Owner, state.hitDirections);
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
        foreach (KeyValuePair<AGameCharacter, HitDirections[]> value in currentAttacksDictionary)
        {
            if (value.Key.Equals(locked))
            {
                anyLocked = true;
                for (int i = 0; attackUISignalers.Length > i; i++)
                {
                    setAttackObject(attackUISignalers[i], value.Value.Contains((HitDirections)i));
                }
            }
            else
            {
                anyOutsideAttack = true;
            }
        }
        middleDanger.SetActive(anyOutsideAttack);
        middleDangerAnimator.Play("Danger");

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
        obj.GetComponent<Image>().color = new Color(on ? 0 : 1, 0,0,1);

    }
}
