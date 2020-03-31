using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralBlock : MonoBehaviour
{
    public bool hasTower = false;

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
        if (Input.GetMouseButtonDown(0))
        {
            if (!hasTower)
            {
                FindObjectOfType<TowerFactory>().AddTower(this);
                hasTower = true;
            }
        }
    }
}
