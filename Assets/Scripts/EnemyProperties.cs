using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public int hitsLeft = 5;
    public int waypointIndex = 0;

    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] ParticleSystem deathParticlePrefab;
    //[SerializeField] AudioClip hitSfx;
    //[SerializeField] AudioClip deathSoundFx;
    //[SerializeField] ParticleSystem goalParticles;
    //[SerializeField] AudioClip reachedGoalSfx;

    EnemySpawner enemies;

    private void Start()
    {
        enemies = GetComponentInParent<EnemySpawner>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        hitParticles.Play();
        hitsLeft--;
        //GetComponent<AudioSource>().PlayOneShot(hitSfx);

        if (hitsLeft == 0)
        {
            ProcessEnemyDeath();
        }

        Destroy(collision.collider.gameObject);
    }

    //Old code from when I used particles as bullets
    //private void OnParticleCollision(GameObject other)
    //{
    //    hitParticles.Play();
    //    hitsLeft--;
    //    GetComponent<AudioSource>().PlayOneShot(hitSfx);

    //    if (hitsLeft == 0)
    //    {
    //        ProcessEnemyDeath();
    //    }
    //}

    public void ProcessEnemyDeath()
    {
        enemies.getEnemies().Remove(this);
        enemies.deadEnemies++;
        enemies.GivePlayerMoney();
        ParticleSystem deathFx = PlayDeathEffects();
        Destroy(deathFx.gameObject, deathFx.main.duration);
        Destroy(gameObject);
    }
    
    private ParticleSystem PlayDeathEffects()
    {
        Vector3 adjustedPos = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
        //AudioSource.PlayClipAtPoint(deathSoundFx, new Vector3(23.5f, 80f, -64.1f));
        ParticleSystem deathFx = Instantiate(deathParticlePrefab, adjustedPos, Quaternion.identity);
        deathFx.Play();
        return deathFx;
    }

    public void SetCurrentLocation()
    {
        transform.position = Pathfinder.path[waypointIndex].transform.position;
        
        if (waypointIndex + 1 == Pathfinder.path.Count)
        {
            ProcessReachingGoal();
        }
    }

    private void ProcessReachingGoal()
    {
        //AudioSource.PlayClipAtPoint(reachedGoalSfx, new Vector3(23.5f, 80f, -64.1f));
        enemies.DamagePlayer();
        SelfDestruct();
    }

    private void SelfDestruct()
    {
        //ParticleSystem goalFx = Instantiate(goalParticles, transform.position, Quaternion.identity);
        //goalFx.Play();
        enemies.getEnemies().Remove(this);
        enemies.deadEnemies++;
        //Destroy(goalFx.gameObject, goalFx.main.duration);
        Destroy(gameObject);
    }
}
