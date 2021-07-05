using System.Collections;
using System.Collections.Generic;
using TowerDefense.Attributes;
using TowerDefense.Control;
using TowerDefense.Core;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense.Combat
{
    public class EnemyActions : MonoBehaviour
    {
        public UnityAction onReachedGoal;

        [SerializeField] float moveDelay = .5f;

        public Coroutine MoveEnemy(EnemyProperties enemy, int moveAmount)
        {
            return StartCoroutine(MoveOverTime(enemy, moveAmount));
        }

        public void AttackTower(EnemyProperties enemy, int attackDmg)
        {
            GameObject closestTower;
            List<GameObject> towerList = transform.parent.GetComponentInChildren<TowerCreator>().towerList;
            closestTower = towerList[0];
            foreach (GameObject tower in towerList)
            {
                if (tower == closestTower) { continue; }

                closestTower = DetermineClosestTower(enemy, closestTower, tower);
            }

            closestTower.GetComponent<NewTower>().ModifyHealth(-attackDmg);
        }

        // Will move the enemy a specified number of spaces with a slight delay between moves.
        private IEnumerator MoveOverTime(EnemyProperties enemy, int moveAmount)
        {
            int spacesMoved = 0;
            while (spacesMoved < moveAmount)
            {
                int waypointIndex = enemy.GetWaypointIndex();
                enemy.transform.position = Pathfinder.path[waypointIndex + 1].transform.position;

                if (enemy.WillReachGoal()) // If the enemy has reached the last block move it and end coroutine.
                {
                    enemy.transform.position = Pathfinder.path[Pathfinder.path.Count - 1].transform.position;
                    onReachedGoal.Invoke();
                    yield break;
                }
                else
                {
                    enemy.transform.position = Pathfinder.path[waypointIndex].transform.position;
                    FaceForward(enemy);
                }
                spacesMoved++;
                yield return new WaitForSeconds(moveDelay);
            }
        }

        private GameObject DetermineClosestTower(EnemyProperties enemy, GameObject closestTower, GameObject tower)
        {
            // The closest tower will be targeted.
            if (Vector3.Distance(enemy.transform.position, tower.transform.position) <
                            Vector3.Distance(enemy.transform.position, closestTower.transform.position))
            {
                closestTower = tower;
            }
            // If the distance between two towers is the same, the tiebreaker is health.
            else if (Vector3.Distance(enemy.transform.position, tower.transform.position) ==
                Vector3.Distance(enemy.transform.position, closestTower.transform.position))
            {
                // The tower with the lowest health will be targeted.
                if (tower.GetComponent<NewTower>().currentHealth < closestTower.GetComponent<NewTower>().currentHealth)
                {
                    closestTower = tower;
                }
                // If the towers have the same health, one will randomly be chosen.
                else if (tower.GetComponent<NewTower>().currentHealth == closestTower.GetComponent<NewTower>().currentHealth)
                {
                    int rand = Random.Range(1, 3);
                    if (rand == 1)
                    {
                        closestTower = tower;
                    }
                }
            }

            return closestTower;
        }

        // Makes sure the enemy is rotated to properly be facing the next block
        private void FaceForward(EnemyProperties enemy)
        {
            var direction = enemy.GetDirection();
            if (direction.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (direction.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (direction.z > 0)
            {
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (direction.z < 0)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
    }
}
