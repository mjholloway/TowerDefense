using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTower : MonoBehaviour, IDamageable
{
    public int currentHealth { get; set; } = 50;
    public int maxHealth { get; set; } = 50;

    [SerializeField] HealthBarManager healthBar;

    public void ModifyHealth(int change)
    {
        int previousHealth = currentHealth;
        currentHealth += change;
        healthBar.ModifyHealthBar(previousHealth, currentHealth, maxHealth);
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
