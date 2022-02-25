using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupProjectile : MonoBehaviour
{
    public GameObject itemPrefab;
    private bool pickedUp = false;

    // Update is called once per frame
    void Update()
    {
        if (pickedUp) return;
        PlayerManager pm = PlayerManager.instance;
        if (pm == null) return;
        if (Vector3.Distance(pm.transform.position, transform.position) < 1f) {
            pickedUp = true;
            PlayerManager.instance.GetPlayerThrowing().AddProjectile(itemPrefab);
            PlayerManager.inventory.PlayPickUpWeapon();
            Destroy(gameObject);
        }
    }
}
