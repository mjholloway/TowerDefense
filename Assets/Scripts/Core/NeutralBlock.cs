using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Core
{
    public class NeutralBlock : MonoBehaviour
    {
        [SerializeField] NeutralBlockManager manager;

        GameObject towerToPlace = null;
        bool hasTower = false;

        private void OnMouseOver()
        {
            if (hasTower) { return; }
            towerToPlace = manager.GetTowerToPlace();

            if (towerToPlace == null) { return; }
            towerToPlace.GetComponent<UnplacedTower>().SnapTower(gameObject);
            if (Input.GetMouseButtonDown(0))
            {
                hasTower = true;
                AddRoots();
                manager.onTowerPlacement.Invoke();
            }
            //if (towerCreator.holdingTower && !hasTower)
            //{
            //    towerCreator.tower.GetComponent<UnplacedTower>().SnapTower(gameObject);

            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        //factory.AddTower(this);
            //        hasTower = true;
            //        AddRoots();
            //        towerCreator.ClearTower();
            //    }
            //}
        }

        private void AddRoots()
        {
            //Simply iterate through the children of this GameObject (which are simply the two different blocks) and disable the first (the normal block)
            //and enable the second (the roots block).
            var children = GetComponentsInChildren<MeshRenderer>(true);
            children[0].gameObject.SetActive(false);
            children[1].gameObject.SetActive(true);
        }

        private void OnMouseExit()
        {
            if (hasTower || towerToPlace == null) { return; }

            towerToPlace.GetComponent<UnplacedTower>().UnSnapTower();
        }
    }
}
