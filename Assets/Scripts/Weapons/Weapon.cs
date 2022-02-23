using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [Header("Weapon Data")]
    [SerializeField] WeaponData weaponData;      //Reference to WeaponData
    [SerializeField] private Transform firingPoint; // Reference to firingPoint
    [SerializeField] public Transform scopePoint;
    public float dropForceForward = 10f;
    public float dropForceUp = 2f;
    float LastShotTime;
    AudioSource shootingFSX;
    private Camera fpsCam;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool loading;
    private float loadingTime = 0f;
    public GameObject dropPrefab;
    private LineRenderer lineRenderer;
    private Recoil recoil;

    private void Start()
    {
        shootingFSX = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();

        lineRenderer = GetComponentInParent<LineRenderer>();

        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
        recoil = GetComponentInParent<Recoil>();
    }

    private void Drop() {
        GameObject drop = Instantiate(dropPrefab);
        drop.transform.position = transform.position;
        drop.transform.rotation = transform.rotation;
        Rigidbody rb = drop.GetComponent<Rigidbody>();
        rb.velocity = GetComponentInParent<CharacterController>().velocity;
        rb.AddForce(GetComponentInParent<Camera>().transform.forward * dropForceForward, ForceMode.Impulse);
        rb.AddForce(GetComponentInParent<Camera>().transform.up * dropForceUp, ForceMode.Impulse);
        Destroy(gameObject);
    }

    private void OnEnable() {
        PlayerShooting.shootInput += Shoot;
        PlayerShooting.reloadInput += Reload;
        loading = true;
        transform.localRotation = Quaternion.AngleAxis(-90, Vector3.right);
    }

    private void OnDisable() {
        PlayerShooting.shootInput -= Shoot;
        PlayerShooting.reloadInput -= Reload;
        loading = false;
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
        if (LastShotTime > (1f / (weaponData.fireRate / 60f)))
            return true;
        else
            return false;
    }

    public void Shoot()
    {
        if(!loading)
        {
            if(CanShoot())
            {
                Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
                RaycastHit hitInfo;
                Vector3 end;
                if(Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hitInfo, weaponData.maxDistance))
                {
                    end = hitInfo.point;
                    hitInfo.rigidbody?.AddForceAtPosition(fpsCam.transform.forward, hitInfo.point);
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    if (damageable == null) { 
                        hitInfo.transform.GetComponentInChildren<IDamageable>();
                    }
                    if (damageable == null) { 
                        hitInfo.transform.GetComponentInParent<IDamageable>();
                    }
                    damageable?.TakeDamage(weaponData.damage);
                    
                }
                else 
                {
                    end = fpsCam.transform.position + fpsCam.transform.forward * weaponData.maxDistance;
                }
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, firingPoint.position);
                lineRenderer.SetPosition(1, end);
                shootingFSX.Play();
                // BulletManager.instance.Shoot(firingPoint.position, end);
                weaponData.currentAmmo--;
                LastShotTime = 0f;
                recoil.RecoilFire();
            }
        }
    }

    private void Update()
    {
        if (loading) {
            //transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, 10 *Time.deltaTime);
            //transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRot, 10 * Time.deltaTime);
            loadingTime += Time.deltaTime;
            if (loadingTime >= 1f) {
                //transform.localPosition = originalPos;
                //transform.localRotation = originalRot;
                loading = false;
                loadingTime = 0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            Drop();
        }
        LastShotTime += Time.deltaTime;
        if (LastShotTime > Time.deltaTime) {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
        }
    }
}
