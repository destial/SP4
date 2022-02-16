using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    // Start is called before the first frame update
    void Start()
    {
        primaryWeapon.SetActive(true);
        secondaryWeapon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            secondaryWeapon.SetActive(false);
            if (secondaryWeapon.GetComponentInChildren<Weapon>() != null) {
                secondaryWeapon.GetComponentInChildren<Weapon>().enabled = false;
            }
            primaryWeapon.SetActive(true);
            if (primaryWeapon.GetComponentInChildren<Weapon>() != null) {
                primaryWeapon.GetComponentInChildren<Weapon>().enabled = true;
            }
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            secondaryWeapon.SetActive(true);
            if (secondaryWeapon.GetComponentInChildren<Weapon>() != null) {
                secondaryWeapon.GetComponentInChildren<Weapon>().enabled = true;
            }
            primaryWeapon.SetActive(false);
            if (primaryWeapon.GetComponentInChildren<Weapon>() != null) {
                primaryWeapon.GetComponentInChildren<Weapon>().enabled = false;
            }
         }
    }

    public GameObject GetActiveSlot() {
        return primaryWeapon.activeSelf ? primaryWeapon : secondaryWeapon;
    }
}
