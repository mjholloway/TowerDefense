using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float secondsBetweenSpawn = 5f;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Text enemyText;
    [SerializeField] AudioClip spawnEnemySfx;

    EnemyProperties[] enemies = new EnemyProperties[0];
    int enemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemyText.text = "0";
        enemies = FindObjectsOfType<EnemyProperties>();
        enemyCount = enemies.Length;
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {        
        while (enemyCount < 10)
        {
            yield return new WaitForSeconds(secondsBetweenSpawn);
            GetComponent<AudioSource>().PlayOneShot(spawnEnemySfx);
            var newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            newEnemy.transform.parent = gameObject.transform;
            enemyCount++;
        }
    }

    public EnemyProperties[] getEnemies()
    {
        return enemies;
    }

    // Update is called once per frame
    void Update()
    {
        enemies = FindObjectsOfType<EnemyProperties>();
        enemyText.text = enemyCount.ToString();
    }
}
