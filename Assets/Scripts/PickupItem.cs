using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public float pickupRange = 3f;
    public GameObject itemPrefab;
    public WeaponMeta weaponMeta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = PlayerManager.player;
        if (player == null) return;

        if (GUIController.instance == null) return;

        Camera camera = player.GetComponentInChildren<Camera>();
        Vector3 rayOrigin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        if (Physics.Raycast(rayOrigin, camera.transform.forward, out RaycastHit hit, pickupRange)) {
            if (hit.transform == transform) {
                GUIController.instance.e.GetComponent<GUIPickup>().item = transform;
            } else if (GUIController.instance.e.GetComponent<GUIPickup>().item == transform) {
                GUIController.instance.e.GetComponent<GUIPickup>().item = null;
            }
        }
    }
}
