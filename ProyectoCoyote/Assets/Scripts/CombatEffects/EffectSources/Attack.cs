using CombatEffect;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class Attack : ATouchCombatEffectSource
{
    [SerializeField] AttackData _attackData;
    [field:SerializeField]public AGameCharacter owner { get; private set; }
    [field: SerializeField] public bool Parreable { get;private set; }
    [field: SerializeField] public HittableTypes HitCheckType { get; private set; }
    AHittableCheck HitCheck;

    [field: SerializeField] public HitDirections[] HitDirections { get; private set; }
    protected override void OnTriggerEnter(Collider other)
    {
        AGameCharacter character = other.GetComponent<AGameCharacter>();
        if (character)
        {
            //Comprueba si el personaje golpeado es golpeable
            if (HitCheck.isHittable(character))
            {
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
    protected void Start()
    {
        setHitCheck(HitCheckType);
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

    public void LoadData(AttackData data)
    {
        setHitCheck(data.HitCheckType);
        data.HitDirections.CopyTo(HitDirections,0);
        effects.Clear();
        foreach (var effect in data.effects)
        {
            effect.setSource(this);
            effects.Add(effect);
        }
        owner.PlayAnimation(data.clip);
    }

}
