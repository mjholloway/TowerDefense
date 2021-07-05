using TowerDefense.Core;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense.Attributes
{
    public class EnemyProperties : MonoBehaviour//, IDamageable
    {
        //TODO: Do these need to be public??
        public int currentHealth { get; set; } = 10;
        public int maxHealth { get; set; } = 10;
        public UnityAction onDeath;

        HealthBarManager healthBar;
        int waypointIndex = 0;

        private void Awake()
        {
            healthBar = GetComponentInChildren<HealthBarManager>();
        }

        public void ModifyHealth(int change)
        {
            int previousHealth = currentHealth;
            currentHealth -= change;
            if (currentHealth < 0)
            {
                Die();
            }
            healthBar.ModifyHealthBar(previousHealth, currentHealth, maxHealth);
        }

        public Vector3 GetDirection()
        {
            return Pathfinder.path[waypointIndex + 1].transform.position - Pathfinder.path[waypointIndex].transform.position;
        }
        public int GetWaypointIndex()
        {
            return waypointIndex;
        }

        public bool WillReachGoal()
        {
            waypointIndex++;
            return waypointIndex + 1 == Pathfinder.path.Count - 1;
        }

        private void Die()
        {
            onDeath.Invoke();
        }
    }
}
