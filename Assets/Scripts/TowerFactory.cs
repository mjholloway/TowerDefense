using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;
    [SerializeField] int towerLimit = 5;

    PlayerHealth player;

    private void Start()
    {
        player = FindObjectOfType<PlayerHealth>();
    }

    public void AddTower(NeutralBlock block)
    {
        if (player.money >= 100)
        {
            InstantiateNewTower(block);
        }
    }

    private void InstantiateNewTower(NeutralBlock block)
    {
        Vector3 towerPos = new Vector3(block.transform.position.x, block.transform.position.y + 10f, block.transform.position.z);
        Tower newTower = Instantiate(towerPrefab, towerPos, Quaternion.identity);
        newTower.transform.parent = gameObject.transform;
        newTower.baseBlock = block;
        player.money -= 100;
    }
}
