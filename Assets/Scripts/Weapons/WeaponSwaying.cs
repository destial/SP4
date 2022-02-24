using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwaying : MonoBehaviour
{
    [Header("Weapon Swaying Settings")]
    [SerializeField] private float smooth = 1;
    [SerializeField] private float multiplier = 1;
    [SerializeField] private float scopeFov = 15;

    private Vector3 original;
    public Vector3 center;

    private Camera fpsCam;
    private float originalFov;
    private float originalLookSpeed;
    private WeaponMeta weaponData;

    private void Start() {
        original = transform.localPosition;
        fpsCam = GetComponentInParent<Camera>();
        originalFov = fpsCam.fieldOfView;
        originalLookSpeed = fpsCam.GetComponentInParent<PlayerMovement>().lookSpeed;
        weaponData = GetComponent<WeaponMeta>();
        scopeFov = 15;
    }

    private void Update()
    {
        if (GameStateManager.Instance.CurrentGameState == GameState.Paused) return;
        if (Input.GetMouseButton(1)) {
            weaponData.isScoping = true;
            Vector3 scope = GetComponent<Weapon>().scopePoint.transform.localPosition;
            if (Vector3.Dot(scope, transform.localPosition) >= 0.7f) transform.localPosition = scope;
            else transform.localPosition = Vector3.Slerp(transform.localPosition, scope, smooth * Time.deltaTime);
            if (fpsCam.fieldOfView - scopeFov <= 0.01f) fpsCam.fieldOfView = scopeFov;
            else fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, scopeFov, smooth * Time.deltaTime);
            fpsCam.GetComponentInParent<PlayerMovement>().lookSpeed = originalLookSpeed * 0.5f;
            GUIController.instance.crosshair.SetActive(false);
        }
        else
        {
            weaponData.isScoping = false;
            if (Vector3.Dot(transform.localPosition, original) >= 0.9f) transform.localPosition = original;
            else transform.localPosition = Vector3.Slerp(transform.localPosition, original, smooth * Time.deltaTime);
            if (originalFov - fpsCam.fieldOfView <= 0.01f) fpsCam.fieldOfView = originalFov;
            else fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, originalFov, smooth * Time.deltaTime);
            fpsCam.GetComponentInParent<PlayerMovement>().lookSpeed = originalLookSpeed;
            GUIController.instance.crosshair.SetActive(true);
        }
        //float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
        //float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

        //Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
        //Quaternion rotationY = Quaternion.AngleAxis(-mouseX, Vector3.up);

        //Quaternion targetRotation = rotationX * rotationY;

        // transform.localRotation = Quaternion.Euler(Vector3.zero);
        //else transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        
    }
}
