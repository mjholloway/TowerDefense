using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense.Core
{
    public class NeutralBlockManager : MonoBehaviour
    {
        public UnityAction onTowerPlacement;
        //public bool canTowerBePlaced = false;

        GameObject tower = null;

        public void SetTowerToPlace(GameObject tower)
        {
            this.tower = tower;
        }

        public GameObject GetTowerToPlace()
        {
            return tower;
        }
    }
}
