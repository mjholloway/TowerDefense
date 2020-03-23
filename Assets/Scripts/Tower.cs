using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform objectToPan;
    [SerializeField] Transform targetEnemy;
    [SerializeField] float attackRange = 29f;

    ParticleSystem particles;

    private void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((targetEnemy) && (Vector3.Distance(objectToPan.position, targetEnemy.position) < attackRange))
        {
            LookAtEnemy();
            ShootEnemy(true);
        }
        else
        {
            ShootEnemy(false);
        }
    }

    private void LookAtEnemy()
    {
            objectToPan.LookAt(targetEnemy);      
    }

    private void ShootEnemy(bool inRange)
    {
        var emissionModule = particles.emission;
        emissionModule.enabled = inRange;
    }

}
