using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    [Header("References")]

    public Camera camera;
    public Transform throwPoint;

    public GameObject throwableObject;

    [Header("Settings")]
    public int totalThrows;
    public float coolDown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;


    private LineRenderer lineRender;

    bool canThrow;
    bool released = false;
    private void Start()
    {
        canThrow = true;
        lineRender = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey))
        {
            lineRender.enabled = true;
        }
        else if (Input.GetKeyUp(throwKey) && canThrow && totalThrows > 0)
        {
            lineRender.enabled = false;
            Throw();
        }
    }

    private void Throw()
    {
        canThrow = false;

        //This part instantiates the throwable object
        GameObject projectile = Instantiate(throwableObject,throwPoint.position,camera.transform.rotation, EntityManager.Instance.gameObject.transform);

        // Physics
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>(); // Gets the throwable object's rigidbody



        Vector3 forceDirection = camera.transform.forward;
        RaycastHit hit;

        if(Physics.Raycast(camera.transform.position,camera.transform.forward, out hit, 500f))
        {
            forceDirection = (hit.point - throwPoint.position).normalized; // Calculates the direction from player's camera to raycast hitpoint
        }

        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

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
