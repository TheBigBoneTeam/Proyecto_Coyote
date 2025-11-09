using System;
using System.Diagnostics;
using UnityEngine;

public class Enemy : AGameCharacter
{
    combatAreaManager combatArea;
    [SerializeField] bool ActiveBeforeFight;
    public combatAreaManager CombatArea { get; private set; }
    bool setredUp;

    public override void Die()
    {
        dieEvent?.Invoke(this);
        GetComponent<EnemyAssetBehaviourRunner>().enabled = false;
        gameObject.SetActive(false);
    }
    public override bool isOtherTeam(AGameCharacter character)
    {
        print(character.name);
        print(character.GetComponent<Enemy>() == null);
        return character.GetComponent<Enemy>() == null;
    }
    public override void getHit(int damage, bool crit = false)
    {
        base.getHit(damage, crit);
    }
    public void setArea(combatAreaManager combatArea)
    {
        CombatArea =combatArea;
    }
    public override void restart()
    {
        if (!setredUp)
        {
            startPos = transform.position;
            setredUp = true;
        }
        base.restart();
        print(name);
        gameObject.SetActive(ActiveBeforeFight);
        GetComponent<EnemyAssetBehaviourRunner>().enabled = false;

    }
    public void activateEnemy(bool active)
    {
        gameObject.SetActive(ActiveBeforeFight ? true:active);
        GetComponent<EnemyAssetBehaviourRunner>().enabled = active;
        if (active)
        {
            GetComponent<EnemyAssetBehaviourRunner>().restart();
        }
    }
}
