using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.Attributes
{
    public class HealthBarManager : MonoBehaviour
    {
        [SerializeField] Image healthBar;
        [SerializeField] Image damageBar;

        public void ModifyHealthBar(int previous, int current, int max)
        {
            healthBar.fillAmount = (float)current / max;
            StartCoroutine(DamageHealth(previous, current, max));
        }

        private IEnumerator DamageHealth(float previous, float current, float max)
        {
            float passedTime = 0f;
            float targetTime = 1f;
            float lerpVal;


            while (passedTime <= targetTime)
            {
                lerpVal = Mathf.Lerp(previous, current, passedTime / targetTime);
                damageBar.fillAmount = lerpVal / max;
                passedTime += Time.deltaTime;
                yield return null;
            }

        }
    }
}
