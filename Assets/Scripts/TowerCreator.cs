using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCreator : MonoBehaviour, IPointerDownHandler
{
    public bool holdingTower = false;
    public GameObject tower;

    [SerializeField] GameObject towerPrefab;
    [SerializeField] PlayerHealth player;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (player.money >= 100)
        {
            holdingTower = true;
            Vector3 mouseVector = new Vector3(Input.mousePosition.x, 10, Input.mousePosition.y);
            tower = Instantiate(towerPrefab, mouseVector, Quaternion.identity);
        }
    }

    public void ClearTower()
    {
        tower.GetComponent<UnplacedTower>().ActivateTower();
        holdingTower = false;
        tower = null;
    }
}
