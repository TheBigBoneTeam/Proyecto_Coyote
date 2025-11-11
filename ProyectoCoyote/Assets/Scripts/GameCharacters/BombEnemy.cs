using CombatEffect;

public class BombEnemy : Enemy
{
    public override bool checkEffect(ACombatEffect effect)
    {
        if(effect.GetType() == typeof(DamageEffect))
        {
            if (invincible)
            {
                return false;
            }
            print(effect.getOwner());
            if(effect.getOwner().GetComponent<Player>()!= null)
            {
                GetComponent<BombEnemyAssetBehaviourRunner>().hitByPlayer();
                return false;
            }
           
        }
        addEffect(effect);
        return true;

    }
}