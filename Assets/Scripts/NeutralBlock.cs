using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralBlock : MonoBehaviour
{
    public bool hasTower = false;

    //[SerializeField] TowerFactory factory;
    [SerializeField] TowerCreator towerCreator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (towerCreator.holdingTower && !hasTower)
        {
            towerCreator.tower.GetComponent<UnplacedTower>().isOverBlock = true;
            towerCreator.tower.transform.position = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);

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
            towerCreator.tower.GetComponent<UnplacedTower>().isOverBlock = false;
        }
    }
}
