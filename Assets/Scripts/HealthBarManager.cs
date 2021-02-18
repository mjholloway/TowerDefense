using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] Image damageBar;

    float currentHealth = 50;

    public void ModifyHealthBar(int current, int max)
    {
        healthBar.fillAmount = (float) current / max;
        StartCoroutine(DamageHealth(current, max));
    }

    private IEnumerator DamageHealth(float current, float max)
    {
        float passedTime = 0f;
        float targetTime = 1f;
        float lerpVal;

        while (passedTime <= targetTime)
        {
            lerpVal = Mathf.Lerp(currentHealth, current, passedTime / targetTime);
            damageBar.fillAmount = lerpVal / max;
            passedTime += Time.deltaTime;
            yield return null;
        }

        currentHealth = current;
    }
}
