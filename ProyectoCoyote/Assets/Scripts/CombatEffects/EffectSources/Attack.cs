using CombatEffect;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class Attack : ATouchCombatEffectSource
{
    [SerializeField] AttackData _attackData;
    [field:SerializeField]public AGameCharacter owner { get; private set; }
    [field: SerializeField] public bool Parreable { get;private set; }
    [field: SerializeField] public HittableTypes HitCheckType { get; private set; }
    AHittableCheck HitCheck;

    [field: SerializeField] public List<HitDirections> HitDirections { get; private set; }

    UnityEvent<AttackState> attackStateEvent;

    protected override void OnTriggerEnter(Collider other)
    {
        AGameCharacter character = other.GetComponent<AGameCharacter>();
        if (character)
        {
            print("triggerCharacetr");
            //Comprueba si el personaje golpeado es golpeable
            if (HitCheck.isHittable(character))
            {
                print("checkeffect");
                character.GetComponent<DamageReceiver>().checkEffectSource(this);
            }

        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadData(_attackData);
        }
    }
    protected void Awake()
    {
        attackStateEvent = new UnityEvent<AttackState>();

    }
    protected void Start()
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
        setHitCheck(data.HitCheckType);
        setParry(false);
        HitDirections.Clear();
        HitDirections.AddRange(data.HitDirections);
        effects.Clear();
        foreach (var effect in data.effects)
        {
            effect.setSource(this);
            effects.Add(effect);
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
