using UnityEngine;
using TowerDefense.Core;
using TowerDefense.Attributes;
using TowerDefense.Combat;
using TowerDefense.Combat.Enemies;

namespace TowerDefense.Control
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] ParticleSystem hitParticles;
        [SerializeField] ParticleSystem deathParticlePrefab;
        [SerializeField] EnemyProperties properties;
        [SerializeField] EnemyIntents enemyIntent;

        EnemySpawner enemies;
        EnemyActions actions;

        private void Awake()
        {
            enemies = GetComponentInParent<EnemySpawner>();
            actions = GetComponentInParent<EnemyActions>();
        }

        private void OnEnable()
        {
            properties.onDeath += ProcessEnemyDeath;
            actions.onReachedGoal += ProcessReachingGoal;
        }

        private void OnDisable()
        {
            properties.onDeath -= ProcessEnemyDeath;
            actions.onReachedGoal -= ProcessReachingGoal;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                StartCoroutine(enemyIntent.GetIntent(properties, actions));
            }
        }

        /* Function to deal with taking hits from the turrets.
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
        } */

        // When the enemy runs out of health this will clean it up and remove it.
        public void ProcessEnemyDeath()
        {
            enemies.getEnemies().Remove(gameObject);
            enemies.deadEnemies++;
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

        // When the enemy reaches the last block it damages the player base.
        public void ProcessReachingGoal()
        {
            //AudioSource.PlayClipAtPoint(reachedGoalSfx, new Vector3(23.5f, 80f, -64.1f));
            // TODO: Add Damage to player!!
            SelfDestruct();
        }

        // Destorys the enemy object when it reaches the player base.
        private void SelfDestruct()
        {
            //ParticleSystem goalFx = Instantiate(goalParticles, transform.position, Quaternion.identity);
            //goalFx.Play();
            enemies.getEnemies().Remove(gameObject);
            enemies.deadEnemies++;
            //Destroy(goalFx.gameObject, goalFx.main.duration);
            Destroy(gameObject);
        }
    }
}

