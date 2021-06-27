using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Attributes;
using TowerDefense.Core;
using UnityEngine;

namespace TowerDefense.Combat
{
    public class NewTower : MonoBehaviour, IDamageable
    {
        public NeutralBlock baseBlock;
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

        public void Attack(EnemyProperties enemy)
        {
            transform.LookAt(enemy.transform);
            ShootEnemy();
        }

        private void ShootEnemy()
        {
            //TODO: This
        }
    }
}
