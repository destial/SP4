using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Target
{
    public static PlayerManager instance { get; private set; }
    public static GameObject player { get; private set; }
    public static InventoryController inventory { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        player = gameObject;
        inventory = player.GetComponentInChildren<InventoryController>();
    }

    public WeaponMeta GetPrimaryWeapon() {
        GameObject weapon = inventory.primaryWeapon;
        return weapon != null ? weapon.GetComponent<WeaponMeta>() : null;
    }

    public WeaponMeta GetSecondaryWeapon() {
        GameObject weapon = inventory.secondaryWeapon;
        return weapon != null ? weapon.GetComponent<WeaponMeta>() : null;
    }

    public WeaponMeta GetCurrentWeapon() {
        GameObject weapon = inventory.GetActiveSlot();
        return weapon != null ? weapon.GetComponent<WeaponMeta>() : null;
    }

    public PlayerThrowing GetPlayerThrowing() {
        return player.GetComponent<PlayerThrowing>();
    }
}
