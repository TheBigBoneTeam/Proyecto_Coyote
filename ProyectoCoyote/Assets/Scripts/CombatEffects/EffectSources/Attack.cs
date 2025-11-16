using BehaviourAPI.UnityToolkit.GUIDesigner.Editor;
using CombatEffect;
using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class Attack : ATouchCombatEffectSource
{
    [SerializeField] protected AttackData _attackData;
    [field: SerializeField] public AGameCharacter owner { get; private set; }
    [field: SerializeField] public bool Parreable { get; private set; }
    [field: SerializeField] public HittableTypes HitCheckType { get; private set; }
    protected AHittableCheck HitCheck;

    [field: SerializeField] public List<HitDirections> HitDirectionsList { get; private set; }

    public UnityEvent<AttackState> attackStateEvent;

    protected override void OnTriggerEnter(Collider other)
    {
        print(HitCheck == null);
        AGameCharacter character = other.GetComponent<AGameCharacter>();
        if (character)
        {
            if (HitCheck == null)
            {
                setHitCheck(HitCheckType);
            }

            //Comprueba si el personaje golpeado es golpeable
            if (this.HitCheck.isHittable(character))
            {
                character.GetComponent<DamageReceiver>().checkEffectSource(this);
            }

        }
    }
    public override void addEffectsToChar(AGameCharacter charac)
    {
        foreach (var effect in effects)
        {
            effect.setOwner(owner);
            charac.checkEffect(effect);
        }
        //if (oneUse)
        //{
        //    destroySource();
        //}
    }
    private void Update()
    {
        // print(HitCheck == null);
    }
    protected virtual void Awake()
    {
        attackStateEvent = new UnityEvent<AttackState>();

    }
    protected virtual void Start()
    {
        owner = GetComponentInParent<AGameCharacter>();
        setHitCheck(HitCheckType);
        HitDirectionsList = new List<HitDirections>();
    }
    public void setParry(bool parry)
    {
        Parreable = parry;
    }
    public virtual void setOwner(AGameCharacter owner)
    {
        this.owner = owner;
        if (owner != null)
        {
            setHitCheck(HitCheckType);
        }
    }
    public void setHitCheck(HittableTypes type)
    {
        HitCheckType = type;
        switch (type)
        {
            case HittableTypes.allCharacters:
                HitCheck = new AllCharacterHittable(owner);
                break;
            case HittableTypes.allCharactersNoMe:
                HitCheck = new AllCharacterNoMeHittable(owner);
                break;
            case HittableTypes.onlyOtherTeam:
                HitCheck = new OnlyOtherTeamHittable(owner);
                break;
        }
    }
    public void setHitDirections(HitDirections[] directions)
    {
        HitDirectionsList.Clear();
        HitDirectionsList.AddRange(directions);
        sendState();

    }
    public void addHitDirection(HitDirections direction)
    {
        if (!HitDirectionsList.Contains(direction))
            HitDirectionsList.Add(direction);
        sendState();

    }
    public void setHitDirection(HitDirections direction)
    {
        HitDirectionsList.Clear();
        HitDirectionsList.Add(direction);
        sendState();

    }
    void sendState()
    {
        attackStateEvent.Invoke(new AttackState(this, owner));

    }
    void sendNullState()
    {
        attackStateEvent.Invoke(new AttackState(null, owner));

    }
    public void subscribeToStateChange(UnityAction<AttackState> response)
    {
        attackStateEvent.AddListener(response);
        // response(new AttackState(HitDirections.ToArray(),owner));

    }

    public void unSubscribeToStateChange(UnityAction<AttackState> response)
    {
        attackStateEvent.RemoveListener(response);
    }
    public void LoadData(AttackData data)
    {
        print("getData");
        if (data == null)
        {
            print("getDataNull");
            HitDirectionsList.Clear();
            effects.Clear();
            sendNullState();
        }
        else
        {
            setHitCheck(data.HitCheckType);
            setParry(data.isParreable);
            HitDirectionsList.Clear();
            HitDirectionsList.AddRange(data.HitDirections);
            effects.Clear();
            foreach (var effect in data.effects)
            {
                effect.setSource(this);
                effects.Add(effect);
            }
            sendState();
        }
    }

    public void restart()
    {
        attackStateEvent.RemoveAllListeners();
    }

    public class AttackState
    {
        public Attack attack;
        public AGameCharacter Owner;

        public AttackState(Attack attack, AGameCharacter character)
        {
            this.attack = attack;
            this.Owner = character;
        }
    }
    public HitDirections getMainDirection()
    {
        if (HitDirectionsList != null && HitDirectionsList.Count != 0)
        {
            if (HitDirectionsList.Contains(HitDirections.Left))
            {
                if (HitDirectionsList.Contains(HitDirections.Rigth))
                {
                    bool b = (UnityEngine.Random.Range(0, 2) == 0);
                    return HitDirections.Back;
                }
                else
                {
                    return HitDirections.Rigth;
                }
            }
            else if (HitDirectionsList.Contains(HitDirections.Rigth))
            {
                
                    return HitDirections.Left;
                
            }else
            {
                return HitDirections.Back;

            }
        }
        else
        {
            return HitDirections.Back;
        }
    }
}
