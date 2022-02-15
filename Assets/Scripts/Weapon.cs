using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [Header("Weapon Data")]
    [SerializeField] WeaponData weaponData;      //Reference to WeaponData
    [SerializeField] private Transform firingPoint; // Reference to firingPoint
    float LastShotTime;
    AudioSource shootingFSX;
    private void Start()
    {
        PlayerShooting.shootInput += Shoot;
        PlayerShooting.reloadInput += Reload;
        shootingFSX = GetComponent<AudioSource>();
    }

    public void Reload()
    {
        if(!weaponData.IsReloading)
        {
            StartCoroutine(StartReloading());
        }
    }

    private IEnumerator StartReloading()
    {
        weaponData.IsReloading = true;

        yield return new WaitForSeconds(weaponData.reloadTime);

        weaponData.currentAmmo = weaponData.magSize;

        weaponData.IsReloading = false;
    }

    private bool CanShoot()
    {
        if (!weaponData.IsReloading && LastShotTime > (1f / (weaponData.fireRate / 60f)))
            return true;
        else
            return false;
    }

public void Shoot()
    {
        if(weaponData.currentAmmo > 0)
        {
            if(CanShoot())
            {
                if(Physics.Raycast(firingPoint.position, firingPoint.forward, out RaycastHit hitInfo, weaponData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(weaponData.damage);
                    shootingFSX.Play();
                }
                weaponData.currentAmmo--;
                LastShotTime = 0;
                OnShoot();
            }
        }
    }

    private void Update()
    {
        LastShotTime += Time.deltaTime;

        Debug.DrawRay(firingPoint.position, firingPoint.forward);
    }

    private void OnShoot()
    {
        
    }
}
