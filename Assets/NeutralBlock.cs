using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralBlock : MonoBehaviour
{
    [SerializeField] GameObject tower;
    [SerializeField] bool hasTower = false;

    TowersEmpty parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = FindObjectOfType<TowersEmpty>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!hasTower)
            {
                CreateTower();
            }
        }
    }

    private void CreateTower()
    {
        Vector3 towerPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10f, gameObject.transform.position.z);
        GameObject newTower = Instantiate(tower, towerPos, Quaternion.identity);
        newTower.transform.parent = parent.transform;
        hasTower = true;
    }
}
