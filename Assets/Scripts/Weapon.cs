using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [Header("Weapon Data")]
    [SerializeField] WeaponData weaponData;      //Reference to WeaponData
    [SerializeField] private Transform firingPoint; // Reference to firingPoint
    [SerializeField] public Transform scopePoint;
    float LastShotTime;
    AudioSource shootingFSX;
    private Camera fpsCam;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool loading;
    private float loadingTime = 0f;

    private void Start()
    {
        shootingFSX = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();

        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    private void OnEnable() {

        PlayerShooting.shootInput += Shoot;
        PlayerShooting.reloadInput += Reload;
        loading = true;
        
        //transform.localPosition += Vector3.up;
        transform.localRotation = Quaternion.AngleAxis(-90, Vector3.right);
    }

    private void OnDisable() {
        PlayerShooting.shootInput -= Shoot;
        PlayerShooting.reloadInput -= Reload;
        loading = false;
        
        //transform.localPosition += Vector3.up;
        //transform.localRotation = Quaternion.AngleAxis(-90, Vector3.right);
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
        if(weaponData.currentAmmo > 0 && !loading)
        {
            if(CanShoot())
            {
                Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
                RaycastHit hitInfo;
                Vector3 end;
                if(Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hitInfo, weaponData.maxDistance))
                {
                    end = hitInfo.point;
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(weaponData.damage);
                    shootingFSX.Play();
                }
                else 
                {
                    end = fpsCam.transform.position + fpsCam.transform.forward * weaponData.maxDistance;
                }
                BulletManager.instance.Shoot(firingPoint.position, end);
                weaponData.currentAmmo--;
                LastShotTime = 0;
                OnShoot();
            }
        }
    }

    private void Update()
    {
        if (loading) {
            //transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, 10 *Time.deltaTime);
            //transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRot, 10 * Time.deltaTime);
            loadingTime += Time.deltaTime;
            if (loadingTime >= 3f) {
                //transform.localPosition = originalPos;
                //transform.localRotation = originalRot;
                loading = false;
                loadingTime = 0f;
            }
        }
        LastShotTime += Time.deltaTime;

        Debug.DrawRay(firingPoint.position, firingPoint.forward);
    }

    private void OnShoot()
    {
        
    }
}
