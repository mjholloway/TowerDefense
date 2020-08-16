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
    [SerializeField] AudioClip hitSfx;
    [SerializeField] AudioClip deathSoundFx;
    [SerializeField] ParticleSystem goalParticles;
    [SerializeField] AudioClip reachedGoalSfx;

    PlayerHealth player;
    EnemySpawner enemies;
    List<Waypoint> path = new List<Waypoint>();

    private void Start()
    {
        enemies = GetComponentInParent<EnemySpawner>();
        Pathfinder pathfinder = FindObjectOfType<Pathfinder>();
        path = pathfinder.GetPath();
    }

    private void OnParticleCollision(GameObject other)
    {
        hitParticles.Play();
        hitsLeft--;
        GetComponent<AudioSource>().PlayOneShot(hitSfx);

        if (hitsLeft == 0)
        {
            ProcessEnemyDeath();
        }
    }

    public void ProcessEnemyDeath()
    {
        enemies.getEnemies().Remove(this);
        enemies.deadEnemies++;
        GiveMoney();
        ParticleSystem deathFx = PlayDeathEffects();
        Destroy(deathFx.gameObject, deathFx.main.duration);
        Destroy(gameObject);
    }

    private void GiveMoney()
    {
        player = FindObjectOfType<PlayerHealth>();
        player.money += 10;
    }
    
    private ParticleSystem PlayDeathEffects()
    {
        AudioSource.PlayClipAtPoint(deathSoundFx, new Vector3(23.5f, 80f, -64.1f));
        ParticleSystem deathFx = Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        deathFx.Play();
        return deathFx;
    }

    public void SetCurrentLocation()
    {
        transform.position = path[waypointIndex].transform.position;
        if (waypointIndex + 1 == path.Count)
        {
            ProcessReachingGoal();
        }
    }

    private void ProcessReachingGoal()
    {
        AudioSource.PlayClipAtPoint(reachedGoalSfx, new Vector3(23.5f, 80f, -64.1f));
        FindObjectOfType<PlayerHealth>().health--;
        SelfDestruct();
    }

    private void SelfDestruct()
    {
        ParticleSystem goalFx = Instantiate(goalParticles, transform.position, Quaternion.identity);
        goalFx.Play();
        enemies.getEnemies().Remove(this);
        Destroy(goalFx.gameObject, goalFx.main.duration);
        Destroy(gameObject);
    }
}
