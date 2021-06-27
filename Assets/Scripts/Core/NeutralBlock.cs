using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Control;
using UnityEngine;

namespace TowerDefense.Core
{
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
                    AddRoots();
                    towerCreator.ClearTower();
                }
            }
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
            if (towerCreator.holdingTower)
            {
                towerCreator.tower.GetComponent<UnplacedTower>().UnSnapTower();
            }
        }
    }
}
