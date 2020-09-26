using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class CubeEditor : MonoBehaviour
{
    Vector2Int gridPos;
    const int gridSize = 10;

    // Update is called once per frame
    void Update()
    {
        SnapToGrid();
        //UpdateLabel();
    }

    private void SnapToGrid()
    {
        transform.position = new Vector3(
            GetGridPos().x * gridSize,
            transform.position.y, 
            GetGridPos().y * gridSize
            );
    }

    public Vector2Int GetGridPos()
    {
        gridPos.x = Mathf.RoundToInt(transform.position.x / gridSize);
        gridPos.y = Mathf.RoundToInt(transform.position.z / gridSize);
        return gridPos;
    }

    //private void UpdateLabel()
    //{
    //    TextMesh textMesh = GetComponentInChildren<TextMesh>();

    //    string labelText = 
    //        waypoint.GetGridPos().x +
    //        "," + 
    //        waypoint.GetGridPos().y;

    //    textMesh.text = labelText;
    //    gameObject.name = "Cube " + labelText;
    //}
}
