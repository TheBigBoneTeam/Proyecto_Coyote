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
    [field:SerializeField]public AGameCharacter owner { get; private set; }
    [field: SerializeField] public bool Parreable { get;private set; }
    [field: SerializeField] public HittableTypes HitCheckType { get; private set; }
   protected AHittableCheck HitCheck;

    [field: SerializeField] public List<HitDirections> HitDirections { get; private set; }

    UnityEvent<AttackState> attackStateEvent;

    protected override void OnTriggerEnter(Collider other)
    {
        print(HitCheck == null);
        AGameCharacter character = other.GetComponent<AGameCharacter>();
        if (character)
        {
            //if(HitCheck == null)
            //{
            //    setHitCheck(HitCheckType);
            //}
          
            //Comprueba si el personaje golpeado es golpeable
            if (this.HitCheck.isHittable(character))
            {
                character.GetComponent<DamageReceiver>().checkEffectSource(this);
            }

        }
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
        HitDirections = new List<HitDirections>();
    }
    public void setParry(bool parry)
    {
        Parreable = parry;
    }
    public void setOwner(AGameCharacter owner)
    {
        this.owner = owner;
        setHitCheck(HitCheckType);
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
        HitDirections.Clear();
        HitDirections.AddRange(directions);
        sendState();

    }
    public void addHitDirection(HitDirections direction)
    {
        if(!HitDirections.Contains(direction))
        HitDirections.Add(direction);
        sendState();

    }
    public void setHitDirection(HitDirections direction)
    {
        HitDirections.Clear();
        HitDirections.Add(direction);
        sendState();

    }
    void sendState()
    {
        attackStateEvent.Invoke(new AttackState(HitDirections.ToArray()));

    }
    public void subscribeToStateChange(UnityAction<AttackState> response)
    {
        attackStateEvent.AddListener(response);
        response(new AttackState(HitDirections.ToArray()));

    }

    public void unSubscribeToStateChange(UnityAction<AttackState> response)
    {
        attackStateEvent.AddListener(response);
    }
    public void LoadData(AttackData data)
    {
        print("getData");
        if (data == null)
        {
            print("getDataNull");
            HitDirections.Clear();
            effects.Clear();
        }
        else
        {
            setHitCheck(data.HitCheckType);
            setParry(data.isParreable);
            HitDirections.Clear();
            HitDirections.AddRange(data.HitDirections);
            effects.Clear();
            foreach (var effect in data.effects)
            {
                effect.setSource(this);
                effects.Add(effect);
            }
        }
        sendState();
    }

 

    public struct AttackState
    {
       public HitDirections[] hitDirections;

        public AttackState(HitDirections[] hitDirections)
        {
            this.hitDirections = hitDirections;
        }
    }
}
