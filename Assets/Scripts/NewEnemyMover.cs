using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewEnemyMover : MonoBehaviour
{
    [SerializeField] float moveDelay = .5f;

    EnemySpawner enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GetComponent<EnemySpawner>();
        StartCoroutine(MoveEnemies());
    }

    private IEnumerator MoveEnemies()
    {
        while (enemies.deadEnemies < enemies.enemyMax)
        {
            foreach (EnemyProperties enemy in enemies.getEnemies().ToList())
            {
                enemy.waypointIndex++;
                enemy.SetCurrentLocation();
            }
            yield return new WaitForSeconds(moveDelay);
        }
    }
}
