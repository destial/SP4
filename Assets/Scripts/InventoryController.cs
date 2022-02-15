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
            primaryWeapon.SetActive(true);
            secondaryWeapon.SetActive(false);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            primaryWeapon.SetActive(false);
            secondaryWeapon.SetActive(true);
        }
    }
}
