using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firingPoint; // Reference to firingPoint
    [SerializeField] public Transform scopePoint;
    [SerializeField] public GameObject muzzleFlash;
    [SerializeField] public GameObject impactEffect;

    public float dropForceForward = 10f;
    public float dropForceUp = 2f;
    float LastShotTime;
    private AudioSource shootingFSX;
    private Camera fpsCam;
    private bool loading;
    private float loadingTime = 0f;
    public GameObject dropPrefab;
    private LineRenderer lineRenderer;
    private Recoil recoil;
    private bool lastLineRender;
    private Vector3 middle = new Vector3(0.5f, 0.5f, 1f);
    private WeaponMeta weaponData;
    private CharacterController cc;
    private AudioSource noAmmoFSX;

    private void Start()
    {
        shootingFSX = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
        noAmmoFSX = GetComponentInParent<InventoryController>().GetComponent<AudioSource>();
        cc = GetComponentInParent<CharacterController>();
        lineRenderer = GetComponentInParent<LineRenderer>();
        lineRenderer.alignment = LineAlignment.View;
        recoil = GetComponentInParent<Recoil>();
        weaponData = GetComponent<WeaponMeta>();
    }

    private void Drop() {
        GameObject drop = Instantiate(dropPrefab);
        drop.transform.position = transform.position;
        drop.transform.rotation = transform.rotation;
        Rigidbody rb = drop.GetComponent<Rigidbody>();
        WeaponMeta meta = drop.GetComponent<WeaponMeta>();
        weaponData.isReloading = false;
        weaponData.isScoping = false;
        meta.Copy(weaponData);
        rb.velocity = cc.velocity;
        rb.AddForce(fpsCam.transform.forward * dropForceForward, ForceMode.Impulse);
        rb.AddForce(fpsCam.transform.up * dropForceUp, ForceMode.Impulse);
        Destroy(gameObject);
    }

    private void OnEnable() {
        PlayerShooting.shootInput += Shoot;
        PlayerShooting.reloadInput += Reload;
        loading = true;
        // transform.localRotation = Quaternion.AngleAxis(-90, Vector3.right);
    }

    private void OnDisable() {
        PlayerShooting.shootInput -= Shoot;
        PlayerShooting.reloadInput -= Reload;
        loading = false;
    }

    public void Reload()
    {
        if(!weaponData.isReloading)
        {
            StartCoroutine(StartReloading());
        }
    }

    private IEnumerator StartReloading()
    {
        if (weaponData.magAmount > 0) {
            weaponData.isReloading = true;
            yield return new WaitForSeconds(weaponData.reloadTime);
            weaponData.magAmount--;
            weaponData.ammo = weaponData.magSize;
            weaponData.isReloading = false;
        }
        yield return null;
    }

    private bool CanShoot()
    {
        return !weaponData.isReloading && LastShotTime > 1f / weaponData.fireRate;
    }

    public IEnumerator MuzzleFlash() {
        GameObject flash = Instantiate(muzzleFlash, firingPoint);
        flash.transform.position = firingPoint.position;
        flash.transform.localPosition = Vector3.zero;
        flash.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.25f);
        flash.GetComponent<ParticleSystem>().Stop();
        Destroy(flash);
    }

    public IEnumerator BulletImpact(RaycastHit hitInfo) {
        GameObject impact = Instantiate(impactEffect);
        ParticleSystem ps = impact.GetComponent<ParticleSystem>();
        impact.transform.forward = hitInfo.normal;
        impact.transform.position = hitInfo.point;
        impact.transform.localScale = Vector3.one;
        ps.Play();
        yield return new WaitForSeconds(0.25f);
        ps.Stop();
    }

    public void Shoot()
    {
        if(!loading)
        {
            if (CanShoot()) {
                LastShotTime = 0f;
                if (weaponData.ammo <= 0) {
                    noAmmoFSX.Play();
                }
                else
                {
                    StartCoroutine(MuzzleFlash());
                    Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(middle);
                    RaycastHit hitInfo;
                    Vector3 end;
                    if(Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hitInfo, weaponData.maxDistance))
                    {
                        end = hitInfo.point;
                        hitInfo.rigidbody?.AddForceAtPosition(fpsCam.transform.forward * weaponData.damage, hitInfo.point);
                        IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                        if (damageable == null) { 
                            hitInfo.transform.GetComponentInChildren<IDamageable>();
                        }
                        if (damageable == null) { 
                            hitInfo.transform.GetComponentInParent<IDamageable>();
                        }
                        if (damageable != null) {
                            damageable?.TakeDamage(weaponData.damage);
                        } else {
                            StartCoroutine(BulletImpact(hitInfo));
                        }
                        Debug.Log("Hit target: " + hitInfo.transform.gameObject.name);
                    }
                    else 
                    {
                        end = fpsCam.transform.position + fpsCam.transform.forward * weaponData.maxDistance;
                    }
                    Debug.Log(end);
                    if (!weaponData.isScoping) {
                        lineRenderer.enabled = true;
                        lineRenderer.SetPosition(0, firingPoint.position);
                        lineRenderer.SetPosition(1, end);
                    }
                    shootingFSX.Play();
                    // BulletManager.instance.Shoot(firingPoint.position, end);
                    weaponData.ammo--;
                    lastLineRender = false;
                    recoil.RecoilFire();
                }
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
        if (GameStateManager.Instance.CurrentGameState != GameState.Gameplay) return;
        if (Input.GetKeyDown(KeyCode.Q)) {
            Drop();
            return;
        }
        
    }

    private void LateUpdate() {
        LastShotTime += Time.deltaTime;
        if (!lastLineRender && LastShotTime > Time.deltaTime) {
            lineRenderer.enabled = false;
            lastLineRender = true;
        }
        lineRenderer.SetPosition(0, firingPoint.position);
    }
}
