using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonEnemy : MonoBehaviour
{
    EnemyActions actions;
    EnemyProperties enemy;

    private void Start()
    {
        actions = GetComponentInParent<EnemyActions>();
        enemy = GetComponent<EnemyProperties>();
        //GetIntent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(GetIntent());
        }
    }

    private IEnumerator GetIntent()
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
