using System.Collections.Generic;
using TowerDefense.Attributes;
using TowerDefense.Combat;
using TowerDefense.Core;
using UnityEngine;

namespace TowerDefense.Control
{
    public class TowerCreator : MonoBehaviour
    {
        public bool holdingTower = false;
        public GameObject tower;
        public List<GameObject> towerList = new List<GameObject>();

        [SerializeField] GameObject towerPrefab;
        [SerializeField] PlayerHealth player;
        [SerializeField] CardActions cardActions;
        [SerializeField] NeutralBlockManager blockManager;

        private void OnEnable()
        {
            blockManager.onTowerPlacement += PlaceTower;
        }

        private void OnDisable()
        {
            blockManager.onTowerPlacement -= PlaceTower;
        }

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
            blockManager.SetTowerToPlace(tower);
            cardActions.SetTower(tower);
        }

        public void PlaceTower()
        {
            tower.GetComponent<UnplacedTower>().ActivateTower(gameObject);
            holdingTower = false;
            tower = null;
            blockManager.SetTowerToPlace(null);
        }
    }
}
