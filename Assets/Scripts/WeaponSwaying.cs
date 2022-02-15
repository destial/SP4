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
    private void Start() {
        original = transform.localPosition;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1)) {
            transform.localPosition = Vector3.Slerp(transform.localPosition, transform.parent.forward, smooth * Time.deltaTime);
            transform.localRotation = Quaternion.identity;
        } else {
            transform.localPosition = Vector3.Slerp(transform.localPosition, original, smooth * Time.deltaTime);
            float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

            Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(-mouseX, Vector3.up);

            Quaternion targetRotation = rotationX * rotationY;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
    }
}
