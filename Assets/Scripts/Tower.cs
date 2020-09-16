using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public enum shotOptions
{
    First = 0,
    Closest = 1,
    Last = 2
}
public class Tower : MonoBehaviour
{
    public NeutralBlock baseBlock;
    public shotOptions shotMode = 0;
    public bool isSelected = false;
    public GameObject rangeIndicator;

    [SerializeField] Transform objectToPan;
    [SerializeField] float attackRange = 29f;

    List<EnemyProperties> enemyList = new List<EnemyProperties>();
    ParticleSystem particles;
    EnemySpawner enemies;
    EnemyProperties targetEnemy;
    float targetDistance = 100f;

    private void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        enemies = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        FindTargetDistance();

        if ((targetEnemy) && (targetDistance < attackRange))
        {
            LookAtEnemy();
            ShootEnemy(true);
        }
        else
        {
            ShootEnemy(false);
        }
    }
    
    private void FindTargetDistance()
    {
        enemyList = enemies.getEnemies();
        DetermineTarget();
    }

    private void DetermineTarget()
    {
        foreach (EnemyProperties enemy in enemyList)
        {
            if (targetEnemy) { targetDistance = Vector3.Distance(targetEnemy.transform.position, gameObject.transform.position); } // set target distance every frame
            float newDistance = Vector3.Distance(enemy.transform.position, gameObject.transform.position);
            if (newDistance > attackRange) { continue; } // don't bother checking enemy if enemy is outside of attack range
            switch (shotMode)
            {
                case shotOptions.First:
                    FindFirstEnemy(newDistance, enemy);
                    continue;
                case shotOptions.Closest:
                    FindClosestEnemy(newDistance, enemy);
                    continue;
                case shotOptions.Last:
                    FindLastEnemy(newDistance, enemy);
                    continue;
            }
        }
    }

    private void FindFirstEnemy(float newDistance, EnemyProperties enemy)
    {
        if (!targetEnemy || targetDistance > attackRange || enemy.waypointIndex < targetEnemy.waypointIndex)
        {
            targetEnemy = enemy;
            targetDistance = newDistance;
        }
    }

    private void FindClosestEnemy(float newDistance, EnemyProperties enemy)
    {
        if (!targetEnemy || newDistance < targetDistance)
        {
            targetEnemy = enemy;
            targetDistance = newDistance;
        }
    }

    private void FindLastEnemy(float newDistance, EnemyProperties enemy)
    {
        if (!targetEnemy || targetDistance > attackRange || enemy.waypointIndex > targetEnemy.waypointIndex)
        {
            targetEnemy = enemy;
            targetDistance = newDistance;
        }
    }

    private void LookAtEnemy()
    {
        objectToPan.LookAt(targetEnemy.transform);    
    }

    private void ShootEnemy(bool inRange)
    {
        var emissionModule = particles.emission;
        emissionModule.enabled = inRange;
    }

    public void OnMouseDown()
    {
        PanelManager.DeactivatePanel();
        PanelManager.ActivatePanel(this);
        rangeIndicator.SetActive(true);
    }
}
