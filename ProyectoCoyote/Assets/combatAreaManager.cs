using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class combatAreaManager : MonoBehaviour
{
 [SerializeField]   Cover[] allCover;
    [SerializeField] Cover[] enemies;
    [SerializeField] List<Enemy> deadEnemies;
    Player _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        deadEnemies = new List<Enemy>();
        foreach (var enemy in enemies)
        {
           // enemy.setCombatArea(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            print(getCoverSpot(out Vector3 pos) == null);

        }
    }

    public void enemyDie(Enemy enemy)
    {
        if (!deadEnemies.Contains(enemy))
        {
            deadEnemies.Add(enemy);
        }
        if(deadEnemies.Count == enemies.Length)
        {
            AreaCompleted();
        }
    }

    private void AreaCompleted()
    {
        throw new NotImplementedException();
    }

    public Cover getCoverSpot(out Vector3 hidePosition)
    {
        Transform objPos;
        Cover[] orderedCovers = allCover.OrderBy((c) => -((c.transform.position - _player.transform.position).sqrMagnitude)).ToArray();
        foreach (var cover in orderedCovers)
        {
            print(cover.name);
            if (cover.getBestPoint(_player.transform, out objPos) >= 0)
            {

                hidePosition = objPos.position;
                return cover;
                

            }
        }
        hidePosition = Vector3.zero;
        return null;
    }
}
