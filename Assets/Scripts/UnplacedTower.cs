using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnplacedTower : MonoBehaviour
{
    public bool isOverBlock = false;

    [SerializeField] Material red;
    [SerializeField] Material green;
    [SerializeField] Material defaultStrawberry;

    Tower tower;
    Ray ray;
    RaycastHit hit;
    MeshRenderer[] towerMeshes;
    Material[] defaultMaterials = new Material[2];

    void Start()
    {
        tower = GetComponent<Tower>();
        InitializePlacementMode();

    }

    //disable tower functionality, disable particle emissions, disable box colliders, and set transparent placement color
    private void InitializePlacementMode()
    {
        tower.enabled = false;
        var emissionModule = GetComponentInChildren<ParticleSystem>().emission;
        emissionModule.enabled = false;
        tower.GetComponent<BoxCollider>().enabled = false;
        towerMeshes = GetComponentsInChildren<MeshRenderer>();
        SetColor(red);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOverBlock)
        {
            PositionAtMouse();
        }
    }

    //raycast to determine position of mouse and place tower at that location every frame
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
        SetColor(defaultStrawberry);
        enabled = false;
    }

    public void SnapTower(GameObject block)
    {
        isOverBlock = true;
        transform.position = new Vector3(block.transform.position.x, block.transform.position.y + 10f, block.transform.position.z);
        SetColor(green);
    }

    public void UnSnapTower()
    {
        isOverBlock = false;
        SetColor(red);
    }

    private void SetColor(Material color)
    {
        foreach (MeshRenderer mesh in towerMeshes)
        {
            mesh.material = color;
        }
    }
}
