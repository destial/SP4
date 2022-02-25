using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Target
{
    public static PlayerManager instance { get; private set; }
    public static GameObject player { get; private set; }
    public static InventoryController inventory { get; private set; }

    private float maxHealth;
    private PlayerThrowing throwing;
    private PlayerMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        player = gameObject;
        inventory = player.GetComponentInChildren<InventoryController>();
        throwing = player.GetComponent<PlayerThrowing>();
        movement = player.GetComponent<PlayerMovement>();
        maxHealth = health;
    }

    public WeaponMeta GetPrimaryWeapon() {
        GameObject weapon = inventory.primaryWeapon;
        return weapon != null ? weapon.GetComponentInChildren<WeaponMeta>() : null;
    }

    public WeaponMeta GetSecondaryWeapon() {
        GameObject weapon = inventory.secondaryWeapon;
        return weapon != null ? weapon.GetComponentInChildren<WeaponMeta>() : null;
    }

    public WeaponMeta GetCurrentWeapon() {
        GameObject weapon = inventory.GetActiveSlot();
        return weapon != null ? weapon.GetComponentInChildren<WeaponMeta>() : null;
    }

    public PlayerThrowing GetPlayerThrowing() {
        return throwing;
    }

    public PlayerMovement GetPlayerMovement() {
        return movement;
    }

    public float GetMaxHealth() {
        return maxHealth;
    }
}
