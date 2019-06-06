using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    public int ammoInMag;
    public bool chambered;
}

[System.Serializable]
public class WeaponSetup
{
    public Transform bulletSpawnPoint;
}

/// <summary>
/// Base class for any weapon in this game.
/// </summary>
public class WeaponBase : Equippable, IFirearm
{
    [Header("Weapon Setup")]
    public WeaponSetup setup;
    [Header("Weapon Stats")]
    public WeaponStats weaponInfo;

    public virtual void Shoot()
    {
        equipSetup.animator.SetTrigger("shoot");
    }

    public virtual void Reload()
    {
        
    }

    public virtual void BarrelTraceRaycast()
    {
        if (setup.bulletSpawnPoint == null)
            return;

        RaycastHit hit;
        if (Physics.Raycast(new Ray(setup.bulletSpawnPoint.position, setup.bulletSpawnPoint.forward), out hit, 10000))
        {
            Debug.DrawLine(setup.bulletSpawnPoint.position, hit.point, Color.magenta);
        }
        else
        {
            Debug.DrawRay(setup.bulletSpawnPoint.position, setup.bulletSpawnPoint.forward * 10000, Color.magenta);
        }
    }
}
