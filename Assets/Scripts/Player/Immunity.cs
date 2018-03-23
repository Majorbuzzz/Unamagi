﻿using UnityEngine;
using System;

public class Immunity : MonoBehaviour
{
    private float timerImmunity = 0f;
    private int delay = 2;
    int counter;
    bool toggle = false;

    public SpriteRenderer SpriteRenderer { get; internal set; }

    public bool NotImmune { get { return timerImmunity <= 0; } }
    
    internal void DoYourThing()
    {
        if (timerImmunity >= Time.deltaTime)
        {
            Flash();
            timerImmunity -= Time.deltaTime;
        }
        else
        {
            timerImmunity = 0;
            SpriteRenderer.enabled = true;
        }

    }

    void Flash()
    {
        if (counter >= delay)
        {
            counter = 0;

            toggle = !toggle;
            if (toggle)
                SpriteRenderer.enabled = true;
            else
                SpriteRenderer.enabled = false;
        }
        else
            counter++;
    }

    internal void Start()
    {
        timerImmunity += 2;
    }
}
