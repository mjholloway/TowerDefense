using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTower : MonoBehaviour
{
    public int towerHealth = 50;

    [SerializeField] HealthBarManager healthBar;

    public void ModifyHealth(int change)
    {
        towerHealth += change;
        healthBar.ModifyHealthBar(towerHealth, 50);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("gotem");
            ModifyHealth(-10);
        }
    }

}
