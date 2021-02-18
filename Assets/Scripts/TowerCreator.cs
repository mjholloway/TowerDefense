using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCreator : MonoBehaviour
{
    public bool holdingTower = false;
    public GameObject tower;

    [SerializeField] GameObject towerPrefab;
    [SerializeField] PlayerHealth player;
    [SerializeField] GameObject towerParent;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) && holdingTower == false)
        {
            holdingTower = true;
            Vector3 mouseVector = new Vector3(Input.mousePosition.x, 10, Input.mousePosition.y);
            tower = Instantiate(towerPrefab, mouseVector, Quaternion.identity);
        }
    }

    public void ClearTower()
    {
        tower.GetComponent<UnplacedTower>().ActivateTower(towerParent);
        holdingTower = false;
        tower = null;
    }
}
