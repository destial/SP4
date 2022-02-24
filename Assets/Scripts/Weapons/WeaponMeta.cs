using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeta : MonoBehaviour
{
    public float damage = 10;
    [HideInInspector] public bool isReloading = false;
    [HideInInspector] public bool isScoping = false;
    public int ammo = 32;
    public int magSize = 32;
    public float reloadTime = 2f;
    public float fireRate = 120f;
    public float maxDistance = 40f;
    public int magAmount = 2;

    public void Copy(WeaponMeta other) {
        damage = other.damage;
        isReloading = other.isReloading;
        ammo = other.ammo;
        magSize = other.magSize;
        reloadTime = other.reloadTime;
        fireRate = other.fireRate;
        maxDistance = other.maxDistance;
        magAmount = other.magAmount;
    }
}
