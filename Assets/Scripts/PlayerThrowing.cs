using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    [Header("References")]

    public Transform camera;
    public Transform throwPoint;
    public GameObject throwableObject;

    [Header("Settings")]
    public int totalThrows;
    public float coolDown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    bool canThrow;
    private void Start()
    {
        canThrow = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && canThrow && totalThrows > 0)
            Throw();
    }

    private void Throw()
    {
        canThrow = false;

        //This part instantiates the throwable object
        GameObject projectile = Instantiate(throwableObject,throwPoint.position,camera.rotation);

        // Physics
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>(); // Gets the throwable object's rigidbody

        Vector3 forceDirection = camera.transform.forward;
        RaycastHit hit;

        if(Physics.Raycast(camera.position,camera.forward, out hit, 500f))
        {
            forceDirection = (hit.point - throwPoint.position).normalized;
        }

        Vector3 forceToAdd = camera.transform.forward * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse); // Impulse so it will only add the force once

        totalThrows--;

        //throwCooldown
        Invoke(nameof(ResetThrow), coolDown);




    }
    
    // Resets the throw
    private void ResetThrow()
    {
        canThrow = true;
    }
}
