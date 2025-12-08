using CombatEffect;
using Services;
using System;
using UnityEngine;

public class HealthSpawner:MonoBehaviour,IHealthSpawner
{
    ObjectPool<HealOrb> orbs;
    [SerializeField] HealOrb orbPrefab;
    [SerializeField] int startingOrbs;
    [SerializeField] float distance;
    public void Instantiate()
    {

    }
    private void Start()
    {

        orbs = new ObjectPool<HealOrb>(orbPrefab, startingOrbs, true);
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToRestart(restart);
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToStateChange(stateChange);

    }

    private void stateChange(object sender, stateData e)
    {

        if (e.currentState == GameState.NonCombat || e.currentState == GameState.Cutscene)
        {
            foreach (HealOrb orb in orbs.Pool)
            {
                orbs.Return(orb);
            }
        }
    }

    private void restart()
    {
        foreach (HealOrb orb in orbs.Pool) {
            orbs.Return(orb);
        }
    }

    public void returnOrb(HealOrb orb)
    {
        orbs.Return(orb);
    }

    public void spawnOrb(Vector3 pos,int health,bool careAboutHealth = true)
    {
        if (health == 0)
        {
            return;
        }
        for (int i = 0; i < health; i++)
        {
            HealOrb orb = orbs.Get();
            orb.setHeal(1);
            orb.transform.position = pos +  new Vector3(UnityEngine.Random.Range(-distance, distance),0, UnityEngine.Random.Range(-distance, distance));
            orb.careAboutMaxHealth = careAboutHealth;
            orb.Active = true;
            
        }

    }
}
