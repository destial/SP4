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
            secondaryWeapon.GetComponentInChildren<Weapon>().enabled = false;
            primaryWeapon.SetActive(true);
            primaryWeapon.GetComponentInChildren<Weapon>().enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            secondaryWeapon.SetActive(true);
            secondaryWeapon.GetComponentInChildren<Weapon>().enabled = true;
            primaryWeapon.SetActive(false);
            primaryWeapon.GetComponentInChildren<Weapon>().enabled = false;
        }
    }
}
