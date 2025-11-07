using System;
using System.Diagnostics;

public class Enemy : AGameCharacter
{
    combatAreaManager combatArea;
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
    public override void restart()
    {
        base.restart();
        
    }
    public void activateEnemy()
    {
        gameObject.SetActive(true);
        GetComponent<EnemyAssetBehaviourRunner>().enabled = true;
    }
}
