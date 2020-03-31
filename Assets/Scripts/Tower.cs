using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public NeutralBlock baseBlock;

    [SerializeField] Transform objectToPan;
    [SerializeField] float attackRange = 29f;

    EnemyProperties[] targetEnemies = new EnemyProperties[0];
    ParticleSystem particles;
    EnemySpawner enemies;
    EnemyProperties closestEnemy;
    
    private void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        enemies = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        float enemyDistance = FindTarget();

        if ((closestEnemy) && (enemyDistance < attackRange))
        {
            LookAtEnemy();
            ShootEnemy(true);
        }
        else
        {
            ShootEnemy(false);
        }
    }

    private float FindTarget()
    {
        float shortestDistance = 100f;
        targetEnemies = enemies.getEnemies();
        foreach (EnemyProperties enemy in targetEnemies)
        {
            shortestDistance = FindClosestEnemy(shortestDistance, enemy);
        }

        return shortestDistance;
    }

    private float FindClosestEnemy(float shortestDistance, EnemyProperties enemy)
    {
        if (enemy)
        {
            float newDistance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
            if (closestEnemy)
            {
                shortestDistance = Vector3.Distance(gameObject.transform.position, closestEnemy.transform.position);
            }
            if (newDistance < shortestDistance)
            {
                shortestDistance = newDistance;
                closestEnemy = enemy;
            }
        }

        return shortestDistance;
    }

    private void LookAtEnemy()
    {
            objectToPan.LookAt(closestEnemy.transform);      
    }

    private void ShootEnemy(bool inRange)
    {
        var emissionModule = particles.emission;
        emissionModule.enabled = inRange;
    }

}
