using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;
    [SerializeField] int towerLimit = 5;

    TowersEmpty parent;
    PlayerHealth player;

    private void Start()
    {
        parent = FindObjectOfType<TowersEmpty>();
        player = GetComponentInChildren<PlayerHealth>();
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
        newTower.transform.parent = parent.transform;
        newTower.baseBlock = block;
        player.money -= 100;
    }

    //private void ReplaceOldestTower(NeutralBlock block)
    //{
    //    Tower newTower = towers.Dequeue();
    //    newTower.baseBlock.hasTower = false;
    //    Vector3 towerPos = new Vector3(block.transform.position.x, block.transform.position.y + 10f, block.transform.position.z);
    //    newTower.transform.position = towerPos;
    //    towers.Enqueue(newTower);
    //    newTower.baseBlock = block;
    //}
}
