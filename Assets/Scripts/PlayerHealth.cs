﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 5f;
    public int money = 100;

    [SerializeField] Text healthText;
    [SerializeField] Text moneyText;
    // Start is called before the first frame update
    void Start()
    {
        healthText.text = health.ToString();
        moneyText.text = "$" + money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();
        moneyText.text = "$" + money.ToString();
    }
}