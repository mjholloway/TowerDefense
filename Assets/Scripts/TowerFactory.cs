using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;
    [SerializeField] int towerLimit = 5;

    TowersEmpty parent;

    private void Start()
    {
        parent = FindObjectOfType<TowersEmpty>();
    }

    public void AddTower(NeutralBlock block)
    {
        if (towerLimit > 0)
        {
            InstantiateNewTower(block);
        }
        else
        {
            print("Tower limit reached");
        }
    }

    private void InstantiateNewTower(NeutralBlock block)
    {
        Vector3 towerPos = new Vector3(block.transform.position.x, block.transform.position.y + 10f, block.transform.position.z);
        Tower newTower = Instantiate(towerPrefab, towerPos, Quaternion.identity);
        newTower.transform.parent = parent.transform;
        towerLimit--;
    }
}
