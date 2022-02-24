using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    private Text ammoText;
    // Start is called before the first frame update
    void Start()
    {
        ammoText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.instance.GetCurrentWeapon() != null) {
            WeaponMeta current = PlayerManager.instance.GetCurrentWeapon();
            if (current.isReloading) {
                ammoText.text = "Reloading...";
            } else {
                ammoText.text = "Ammo: " + current.ammo + " / " + current.magSize + "\nMags Left: " + current.magAmount;
            }
        } else {
            ammoText.text = "";
        }

    }
}
