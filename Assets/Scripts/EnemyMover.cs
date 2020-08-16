using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    //[SerializeField] float moveDelay = .5f;
    //[SerializeField] ParticleSystem goalParticles;
    //[SerializeField] AudioClip reachedGoalSfx;

    //EnemySpawner enemies;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    enemies = GetComponentInParent<EnemySpawner>();
    //    Pathfinder pathfinder = FindObjectOfType<Pathfinder>();
    //    var path = pathfinder.GetPath();
    //    StartCoroutine(FollowPath(path));
    //}

    //public IEnumerator FollowPath(List<Waypoint> path)
    //{
    //    foreach (Waypoint waypoint in path)
    //    {
    //        transform.position = waypoint.transform.position;
    //        yield return new WaitForSeconds(moveDelay);
    //    }
    //    ProcessReachingGoal();
    //}

    //public void ProcessReachingGoal()
    //{
    //    AudioSource.PlayClipAtPoint(reachedGoalSfx, new Vector3(23.5f, 80f, -64.1f));
    //    FindObjectOfType<PlayerHealth>().health--;
    //    SelfDestruct();    
    //}

    //private void SelfDestruct()
    //{
    //    ParticleSystem goalFx = Instantiate(goalParticles, transform.position, Quaternion.identity);
    //    goalFx.Play();
    //    enemies.getEnemies().Remove(gameObject);
    //    Destroy(goalFx.gameObject, goalFx.main.duration);
    //    Destroy(gameObject);
    //}
}
