using CombatEffect;
using UnityEngine;

public class HealthSpawner:MonoBehaviour,IHealthSpawner
{
    ObjectPool<HealOrb> orbs;
    [SerializeField] HealOrb orbPrefab;
    [SerializeField] int startingOrbs;
    public void Instantiate()
    {

    }
    private void Start()
    {

        orbs = new ObjectPool<HealOrb>(orbPrefab, startingOrbs, true);
    }

    public void returnOrb(HealOrb orb)
    {
        orbs.Return(orb);
    }

    public void spawnOrb(Vector3 pos,int health)
    {
        if (health == 0)
        {
            return;
        }
        HealOrb orb = orbs.Get();
        orb.setHeal(health);
        orb.transform.position = pos;
        orb.Active = true;

    }
}
