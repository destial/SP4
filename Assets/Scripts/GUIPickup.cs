using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPickup : MonoBehaviour
{
    [HideInInspector] public Transform item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = PlayerManager.player;
        if (player == null) return;

        if (item != null && Vector3.Distance(item.position, player.transform.position) <= item.GetComponent<PickupItem>().pickupRange) {
            
            GameObject currentWeapon = player.GetComponentInChildren<InventoryController>().GetActiveSlot();
            if (Input.GetKeyDown(KeyCode.E) && currentWeapon.GetComponentInChildren<Weapon>() == null) {
                Instantiate(item.gameObject.GetComponent<PickupItem>().itemPrefab, currentWeapon.transform);
                Destroy(item.gameObject);
            }
        } else {
            //GetComponent<Text>().enabled = false;
        }
    }

    void OnGUI() {
        if (item == null) return;
        GameObject player = PlayerManager.player;
        if (player == null) return;
        Camera camera = player.GetComponentInChildren<Camera>();
        //GetComponent<Text>().enabled = true
        Vector3 screen = camera.WorldToScreenPoint(item.position);
        Rect rect = new Rect(screen, new Vector2(150, 30));
        GUI.Box(rect, "Press E to equip");
        gameObject.transform.position = screen;
    }
}
