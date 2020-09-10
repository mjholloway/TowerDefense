using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnplacedTower : MonoBehaviour
{
    public bool isOverBlock = false;

    Tower tower;
    Ray ray;
    RaycastHit hit;

    void Start()
    {
        tower = GetComponent<Tower>();
        tower.enabled = false;
        var emissionModule = GetComponentInChildren<ParticleSystem>().emission;
        emissionModule.enabled = false;
        tower.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOverBlock)
        {
            PositionAtMouse();
        }
    }

    private void PositionAtMouse()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 9, QueryTriggerInteraction.Ignore))
        {
            transform.position = hit.point;
        }
    }

    public void ActivateTower()
    {
        tower.enabled = true;
        tower.GetComponent<BoxCollider>().enabled = true;
        this.enabled = false;
    }
}
