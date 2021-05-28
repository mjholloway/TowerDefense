using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCreator : MonoBehaviour
{
    public bool holdingTower = false;
    public GameObject tower;
    public List<GameObject> towerList = new List<GameObject>();

    [SerializeField] GameObject towerPrefab;
    [SerializeField] PlayerHealth player;
    [SerializeField] CardActions cardActions;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) && holdingTower == false)
        {
            CreateTower();
        }
    }

    private void CreateTower()
    {
        holdingTower = true;
        Vector3 mouseVector = new Vector3(Input.mousePosition.x, 10, Input.mousePosition.y);
        tower = Instantiate(towerPrefab, mouseVector, Quaternion.identity);
        towerList.Add(tower);
        cardActions.SetTower(tower);
    }

    public void ClearTower()
    {
        tower.GetComponent<UnplacedTower>().ActivateTower(gameObject);
        holdingTower = false;
        tower = null;
    }
}
