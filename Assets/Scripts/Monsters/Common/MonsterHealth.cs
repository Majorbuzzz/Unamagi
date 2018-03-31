﻿using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour
{
    private Image healthBar;
    private float CurrentValue;
    public float Max;

    private void Start()
    {
        healthBar = GetComponent<Image>();
        CurrentValue = Max;
    }

    void Update()
    {
        healthBar.fillAmount = CurrentValue / Max;
    }

    public void ChangeValue(float value)
    {
        CurrentValue += value;      
    }

    public bool IsDead
    {
        get
        {
            return CurrentValue <= 0;
        }
    }
}

