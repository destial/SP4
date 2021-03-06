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
    private bool loading;
    private float loadingTime = 0f;
    public GameObject dropPrefab;
    private LineRenderer lineRenderer;
    private Recoil recoil;
    private bool lastLineRender;
    private Vector3 middle = new Vector3(0.5f, 0.5f, 1f);
    private WeaponMeta weaponData;

    private void Start()
    {
        shootingFSX = GetComponent<AudioSource>();
        lineRenderer = PlayerManager.inventory.GetLineRenderer();
        lineRenderer.alignment = LineAlignment.View;
        recoil = GetComponentInParent<Recoil>();
        weaponData = GetComponent<WeaponMeta>();
    }

    private void Drop() {
        GameObject drop = Instantiate(dropPrefab, EntityManager.Instance.transform);
        drop.transform.position = transform.position;
        drop.transform.rotation = transform.rotation;
        Rigidbody rb = drop.GetComponent<Rigidbody>();
        WeaponMeta meta = drop.GetComponent<WeaponMeta>();
        weaponData.isReloading = false;
        weaponData.isScoping = false;
        meta.Copy(weaponData);
        rb.velocity = PlayerManager.instance.GetCharacterController().velocity;
        rb.AddForce(PlayerManager.instance.GetCamera().transform.forward * dropForceForward, ForceMode.Impulse);
        rb.AddForce(PlayerManager.instance.GetCamera().transform.up * dropForceUp, ForceMode.Impulse);
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
                    PlayerManager.inventory.PlayOutOfAmmoAudio();
                }
                else
                {
                    StartCoroutine(MuzzleFlash());
                    Vector3 rayOrigin = PlayerManager.instance.GetCamera().ViewportToWorldPoint(middle);
                    RaycastHit hitInfo;
                    Vector3 end;
                    if(Physics.Raycast(rayOrigin, PlayerManager.instance.GetCamera().transform.forward, out hitInfo, weaponData.maxDistance))
                    {
                        end = hitInfo.point;
                        hitInfo.rigidbody?.AddForceAtPosition(PlayerManager.instance.GetCamera().transform.forward * weaponData.damage, hitInfo.point);
                        IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                        if (damageable == null) { 
                            hitInfo.transform.GetComponentInChildren<IDamageable>();
                        }
                        if (damageable == null) { 
                            hitInfo.transform.GetComponentInParent<IDamageable>();
                        }
                        if (damageable != null) {
                            damageable?.TakeDamage(weaponData.damage);
                        } else if (hitInfo.collider.GetComponent<Grenade>() != null) {
                            Grenade grenade = hitInfo.collider.GetComponent<Grenade>();
                            grenade.Explode();
                        } else if (hitInfo.collider.GetComponent<Pipebomb>() != null) {
                            Pipebomb pipebomb = hitInfo.collider.GetComponent<Pipebomb>();
                            pipebomb.Explode();
                        } else {
                            StartCoroutine(BulletImpact(hitInfo));
                        }
                    }
                    else 
                    {
                        end = PlayerManager.instance.GetCamera().transform.position + PlayerManager.instance.GetCamera().transform.forward * weaponData.maxDistance;
                    }
                    if (!weaponData.isScoping) {
                        lineRenderer.enabled = true;
                        lineRenderer.SetPosition(0, firingPoint.position);
                        lineRenderer.SetPosition(1, end);
                    }
                    shootingFSX.Play();
                    weaponData.ammo--;
                    lastLineRender = false;
                    recoil.RecoilFire(weaponData);
                }
            }
        }
    }

    private void Update()
    {
        if (loading) {
            loadingTime += Time.deltaTime;
            if (loadingTime >= 1f) {
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

    void LateUpdate() {
        LastShotTime += Time.deltaTime;
        if (!lastLineRender && LastShotTime > Time.deltaTime) {
            lineRenderer.enabled = false;
            lastLineRender = true;
        }
        lineRenderer.SetPosition(0, firingPoint.position);
    }
}
