using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    public GameObject meleeHolder;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;
    public AudioClip pickupItem;
    public AudioClip outOfAmmo;
    // Start is called before the first frame update
    void Start()
    {
        primaryWeapon.SetActive(true);
        secondaryWeapon.SetActive(false);
        meleeHolder.SetActive(false);
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!meleeHolder.GetComponent<Melee>().canAttack) return;
        if (Input.GetKeyDown(KeyCode.Alpha1) && !primaryWeapon.activeSelf) {
            secondaryWeapon.SetActive(false);
            if (secondaryWeapon.GetComponentInChildren<Weapon>() != null) {
                secondaryWeapon.GetComponentInChildren<Weapon>().enabled = false;
            }
            primaryWeapon.SetActive(true);
            if (primaryWeapon.GetComponentInChildren<Weapon>() != null) {
                primaryWeapon.GetComponentInChildren<Weapon>().enabled = true;
            }
            meleeHolder.SetActive(false);
            lineRenderer.enabled = false;
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && !secondaryWeapon.activeSelf) {
            secondaryWeapon.SetActive(true);
            if (secondaryWeapon.GetComponentInChildren<Weapon>() != null) {
                secondaryWeapon.GetComponentInChildren<Weapon>().enabled = true;
            }
            primaryWeapon.SetActive(false);
            if (primaryWeapon.GetComponentInChildren<Weapon>() != null) {
                primaryWeapon.GetComponentInChildren<Weapon>().enabled = false;
            }
            meleeHolder.SetActive(false);
            lineRenderer.enabled = false;
        } else if (Input.GetKeyDown(KeyCode.Alpha3) && !meleeHolder.activeSelf) {
            secondaryWeapon.SetActive(false);
            if (secondaryWeapon.GetComponentInChildren<Weapon>() != null) {
                secondaryWeapon.GetComponentInChildren<Weapon>().enabled = false;
            }
            primaryWeapon.SetActive(false);
            if (primaryWeapon.GetComponentInChildren<Weapon>() != null) {
                primaryWeapon.GetComponentInChildren<Weapon>().enabled = false;
            }
            meleeHolder.SetActive(true);
            lineRenderer.enabled = false;
        }
    }

    public GameObject GetActiveSlot() {
        return primaryWeapon.activeSelf ? primaryWeapon : secondaryWeapon.activeSelf ? secondaryWeapon : null;
    }

    public void PlayOutOfAmmoAudio() {
        audioSource.PlayOneShot(outOfAmmo);
    }

    public void PlayPickUpWeapon() {
        audioSource.PlayOneShot(pickupItem);
    }

    public LineRenderer GetLineRenderer() {
        return lineRenderer;
    }
}
