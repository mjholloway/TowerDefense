using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    TMP_Dropdown dropdown;

    private void Start()
    {
        gameObject.SetActive(false);
        dropdown = GetComponentInChildren<TMP_Dropdown>();
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
                        panel.currentTower.rangeIndicator.gameObject.SetActive(false);
                    }
                }
                else
                {
                    gameObject.SetActive(false);
                    panel.currentTower.rangeIndicator.gameObject.SetActive(false);
                }
            }
        }
    }

    public static void ActivatePanel(Tower tower)
    {
        panel.gameObject.SetActive(true);
        panel.currentTower = tower;
        panel.dropdown.SetValueWithoutNotify((int)panel.currentTower.shotMode);
    }

    public static void DeactivatePanel()
    {
        if (panel.gameObject.activeSelf == true)
        {
            panel.currentTower.rangeIndicator.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
        }
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
    
