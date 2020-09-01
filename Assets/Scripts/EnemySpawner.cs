using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public int deadEnemies = 0;
    public int enemyMax = 10;

    [SerializeField] float secondsBetweenSpawn = 5f;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] AudioClip spawnEnemySfx;

    List<EnemyProperties> enemies = new List<EnemyProperties>();
    int enemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {        
        while (enemyCount < enemyMax)
        {
            yield return new WaitForSeconds(secondsBetweenSpawn);
            GetComponent<AudioSource>().PlayOneShot(spawnEnemySfx);
            var newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemies.Add(newEnemy.GetComponent<EnemyProperties>());
            newEnemy.transform.parent = gameObject.transform;
            enemyCount++;
            
        }
    }

    public List<EnemyProperties> getEnemies()
    {
        return enemies;
    }
}
