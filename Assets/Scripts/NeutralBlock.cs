using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralBlock : MonoBehaviour
{
    public bool hasTower = false;

    //[SerializeField] TowerFactory factory;
    [SerializeField] TowerCreator towerCreator;

    private void OnMouseOver()
    {
        if (towerCreator.holdingTower && !hasTower)
        {
            towerCreator.tower.GetComponent<UnplacedTower>().SnapTower(gameObject);

            if (Input.GetMouseButtonDown(0))
            {
                //factory.AddTower(this);
                hasTower = true;
                towerCreator.ClearTower();
            }
        }
    }

    private void OnMouseExit()
    {
        if (towerCreator.holdingTower)
        {
            towerCreator.tower.GetComponent<UnplacedTower>().UnSnapTower();
        }
    }
}
