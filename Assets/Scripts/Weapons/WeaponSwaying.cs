using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwaying : MonoBehaviour
{
    [Header("Weapon Swaying Settings")]
    [SerializeField] private float smooth = 8;
    [SerializeField] private float multiplier = 6;

    private Vector3 original;
    public Vector3 center;

    private Camera fpsCam;
    private float originalFov;
    private float originalLookSpeed;
    private void Start() {
        original = transform.localPosition;
        fpsCam = GetComponentInParent<Camera>();
        originalFov = fpsCam.fieldOfView;
        originalLookSpeed = fpsCam.GetComponentInParent<PlayerMovement>().lookSpeed;
    }

    private void Update()
    {
        if (GameStateManager.Instance.CurrentGameState == GameState.Paused) return;
        if (Input.GetMouseButton(1)) {
            Vector3 scope = GetComponent<Weapon>().scopePoint.transform.localPosition;
            transform.localPosition = Vector3.Slerp(transform.localPosition, scope, smooth * Time.deltaTime);
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, 15, smooth * Time.deltaTime);
            fpsCam.GetComponentInParent<PlayerMovement>().lookSpeed = originalLookSpeed * 0.5f;
            //GUIController.instance.crosshair.SetActive(false);
        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, original, smooth * Time.deltaTime);
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, originalFov, smooth * Time.deltaTime);
            fpsCam.GetComponentInParent<PlayerMovement>().lookSpeed = originalLookSpeed;
            //GUIController.instance.crosshair.SetActive(true);
        }
        float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

        Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(-mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        
    }
}
