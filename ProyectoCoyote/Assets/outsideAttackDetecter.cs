using NUnit.Framework;
using Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class outsideAttackDetecter : MonoBehaviour
{

    Attack currentOutsideAttack;
    List<Enemy> allAttacks;
    DefenseAttackUIIndicator defenseAttackUIIndicator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        allAttacks = new List<Enemy>();
        defenseAttackUIIndicator = GetComponent<DefenseAttackUIIndicator>();
        //ResetEnemies(FindObjectsByType<Enemy>(FindObjectsSortMode.None));
    }
    private void Start()
    {
        
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeCombatAreaChange(combatAreaChange);
    }

    private void combatAreaChange(combatAreaManager manager, WaveData data)
    {
        print(data == null);
        print("areachange");
        ResetEnemies(data.enemies);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ResetEnemies(Enemy[] enemies)
    {
        print("resetEnemies");

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponentInChildren<Attack>().unSubscribeToStateChange(detectOutsideAttacks);
                if (enemies[i].GetComponent<Gun>())
                {
                    enemies[i].GetComponent<Gun>().unSubscribeToShoot(detectOutsideAttacks);
                }
            }
        }
        allAttacks.Clear();
        foreach (Enemy enemy in enemies)
        {
            Attack a = enemy.GetComponentInChildren<Attack>();
            allAttacks.Add(enemy);
            a.subscribeToStateChange(detectOutsideAttacks);
            if (enemy.GetComponent<Gun>())
            {
                print("hasGun");
                enemy.GetComponent<Gun>().subscribeToShoot(detectOutsideAttacks);
            }
        }
        
    }
    public void detectOutsideAttacks(Attack.AttackState state )
    {
        if (currentOutsideAttack == null)
        {
            if (state.hitDirections.Length == 0 || !state.hitDirections.Contains(HitDirections.Outside))
            {
                return;
            }
            defenseAttackUIIndicator.OutsideAttackChange(state);
        }
    }
}
