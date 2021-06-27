using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Combat
{
    public interface IDamageable
    {
        int currentHealth { get; set; }
        int maxHealth { get; set; }

        void ModifyHealth(int change);
    }
}
