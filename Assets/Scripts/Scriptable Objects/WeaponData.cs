using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName ="Weapon/Gun")]
public class WeaponData : ScriptableObject
{

    [Header("Weapon info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reload")]
    public int currentAmmo = 10;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    [HideInInspector] // Hide this in inspector
    public bool IsReloading;
}
