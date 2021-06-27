using System.Collections;
using System.Collections.Generic;
using TowerDefense.Attributes;
using UnityEngine;

namespace TowerDefense.Combat
{
    public class CardActions : MonoBehaviour
    {
        GameObject tower;

        public void SetTower(GameObject towerObject)
        {
            tower = towerObject;
        }

        public void BasicAttack(int damage)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.TryGetComponent(out EnemyProperties enemy))
                {
                    tower.GetComponent<NewTower>().Attack(enemy);
                    enemy.ModifyHealth(damage);
                }

            }
        }
    }
}
