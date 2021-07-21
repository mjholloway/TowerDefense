using System.Collections;
using UnityEngine;
using TowerDefense.Attributes;

namespace TowerDefense.Combat.Enemies
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Enemies/Create New Enemy")]
    public class EnemyIntents : ScriptableObject
    {
        [SerializeField] Intents[] enemyIntents = null;

        [System.Serializable]
        class Intents
        {
            public EnemyActionTypes[] actionTypes;
            public int[] actionValues;
        }

        public IEnumerator GetIntent(EnemyProperties enemy, EnemyActions actions)
        {
            actions.MoveEnemy(enemy, 1);
            yield return null;
            //int rand = Random.Range(0, enemyIntents.Length);
            //Intents thisIntent = enemyIntents[rand];
            //for (int i = 0; i < thisIntent.actionTypes.Length; i++)
            //{
            //    switch (thisIntent.actionTypes[i])
            //    {
            //        case EnemyActionTypes.Attack:
            //            actions.AttackTower(enemy, thisIntent.actionValues[i]);
            //            break;
            //        case EnemyActionTypes.Move:
            //            yield return actions.MoveEnemy(enemy, thisIntent.actionValues[i]);
            //            break;
            //    }
            //}
        }

    }
}
