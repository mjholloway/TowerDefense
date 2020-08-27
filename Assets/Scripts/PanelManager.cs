using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelManager : MonoBehaviour
{
    public static PanelManager panel
    {
        get
        {
            return Resources.FindObjectsOfTypeAll<PanelManager>()[0];
        }
    }
    public Tower currentTower;

    bool hasBeenActivated = false;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        CheckToDisable();
    }

    private void CheckToDisable()
    {
        if (gameObject.activeSelf == true && Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) //is mouse over the panel?
            {
                //check if mouse is over a tower gameobject and remove panel if not
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.transform.gameObject.GetComponent<Tower>() == null)
                    {
                        gameObject.SetActive(false);
                    }
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public static void ActivatePanel(Tower tower)
    {
        panel.gameObject.SetActive(true);
        panel.currentTower = tower;
    }

    public void OnDropdownChange(int value)
    {
        switch (value)
        {
            case 0:
                panel.currentTower.shotMode = shotOptions.First;
                return;
            case 1:
                panel.currentTower.shotMode = shotOptions.Closest;
                return;
            case 2:
                panel.currentTower.shotMode = shotOptions.Last;
                return;
        }
    }
}
    
