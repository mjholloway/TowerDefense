﻿using System;
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
    [SerializeField] GameObject stillFace;
    [SerializeField] GameObject shootingFace;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate = .5f;

    List<EnemyProperties> enemyList = new List<EnemyProperties>();
    ParticleSystem particles;
    EnemySpawner enemies;
    EnemyProperties targetEnemy;
    float targetDistance = 100f;
    bool isShooting = false;
    Coroutine coroutine;

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
        //var emissionModule = particles.emission;
        //emissionModule.enabled = inRange;

        //Start face coroutine if an enemy is in range and it has not already been started, and stop the corotine if an enemy is not in range
        if (inRange)
        {
            if (!isShooting)
            {
                coroutine = StartCoroutine(ChangeFace());
            }
        }
        else
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        isShooting = inRange;
    }

    private IEnumerator ChangeFace()
    {
        while (true)
        {
            CreateBullet();
            EnableShootingFace();
            yield return new WaitForSeconds(.25f);
            EnableStillFace();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void CreateBullet()
    {
        //bullet movement is handled in BulletMover script
        if (targetEnemy)
        {
            Vector3 bulletInstLocation = FindBulletLocation();
            var newBullet = Instantiate(bulletPrefab, bulletInstLocation, transform.rotation);
            newBullet.GetComponent<BulletMover>().SetTarget(targetEnemy);
            newBullet.transform.parent = transform;
        }
    }

    private Vector3 FindBulletLocation()
    {
        // Using a circle around the tower, calculate where the bullet should be created by converting the polar coordinates into cartesian coordinates.
        // Note that for the purposes of calculation the x and z coordinates represent the y and x coordinates respectively. Sin is used for x, and Cos for z
        // despite the fact that x would normally be calculated with cos and y would be calculated with sin. This is because the z axis in scene corresponds to
        // the cartesian x axis while the x axis in scene corresponds to the cartesian y axis with respect to the gameobject orientation.
        float radius = 4f;
        float angle = transform.Find("Strawberry Objects").rotation.eulerAngles.y * (Mathf.PI/180);
        float xCenter = transform.position.x;
        float zCenter = transform.position.z;

        float bulletX = xCenter + (radius * Mathf.Sin(angle));
        float bulletZ = zCenter + (radius * Mathf.Cos(angle));

        return new Vector3(bulletX, transform.position.y + 4.5f, bulletZ); 
    }

    public void OnMouseDown()
    {
        PanelManager.DeactivatePanel();
        PanelManager.ActivatePanel(this);
        rangeIndicator.SetActive(true);
    }

    void EnableShootingFace()
    {
        stillFace.SetActive(false);
        shootingFace.SetActive(true);
    }

    void EnableStillFace()
    {
        stillFace.SetActive(true);
        shootingFace.SetActive(false);
    }
}
