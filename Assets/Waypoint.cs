using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    Vector2Int gridPos;
    const int gridSize = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int GetGridSize()
    {
        return gridSize;
    }

    public Vector2Int GetGridPos()
    {
        gridPos.x = Mathf.RoundToInt(transform.position.x / gridSize) * gridSize;
        gridPos.y = Mathf.RoundToInt(transform.position.z / gridSize) * gridSize;
        return gridPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
