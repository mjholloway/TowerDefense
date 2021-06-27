using System.Collections;
using TowerDefense.Attributes;
using UnityEngine;

namespace TowerDefense.Combat
{
    public class SpoonEnemy : MonoBehaviour, IActionable
    {
        EnemyActions actions;
        EnemyProperties enemy;

        private void Awake()
        {
            actions = GetComponentInParent<EnemyActions>();
            enemy = GetComponent<EnemyProperties>();
            //GetIntent();
        }

        public IEnumerator GetIntent()
        {
            int rand = Random.Range(1, 4);
            switch (rand)
            {
                case 1:
                    actions.AttackTower(enemy, 10);
                    break;
                case 2:
                    Coroutine wait = actions.MoveEnemy(enemy, 1);
                    yield return wait;
                    actions.AttackTower(enemy, 5);
                    break;
                case 3:
                    actions.MoveEnemy(enemy, 2);
                    break;
            }
        }
    }
}
