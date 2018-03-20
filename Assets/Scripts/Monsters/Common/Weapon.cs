using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    internal event Action WeaponHit;

    public void TriggerWeaponHit()
    {
        if (WeaponHit != null)
            WeaponHit();        
    }
}
