using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        gameObject.SetActive(false);
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
    
