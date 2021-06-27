using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Core
{
    public class Waypoint : MonoBehaviour
    {
        public bool isExplored = false;
        public Waypoint exploredFrom;

        public void SetTopColor(Color color)
        {
            MeshRenderer topMesh = transform.Find("Top").GetComponent<MeshRenderer>();
            topMesh.material.color = color;
        }
    }
}
