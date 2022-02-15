using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Throwable", menuName = "Throwable/ThrowData")]
public class ThrowableData : ScriptableObject
{
    [Header("Throwable info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reload")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    [HideInInspector] // Hide this in inspector
    public bool IsReloading;
}
