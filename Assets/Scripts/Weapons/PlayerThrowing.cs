using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    [Header("References")]

    public Camera camera;
    public Transform throwPoint;

    public GameObject throwableObject;
    
    public List<GameObject> throwablesPrefabs;

    private Dictionary<GameObject, int> projectiles;

    [Header("Settings")]
    public float coolDown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse3;
    public float throwForce;
    public float throwUpwardForce;

    private LayerMask collideableLayers;
    private LineRenderer lineRender;
    private CharacterController cc;

    bool canThrow;
    bool released = true;

    int projectileSelection = 0;

    private void Start()
    {
        projectiles = new Dictionary<GameObject, int>();
        foreach (GameObject throwable in throwablesPrefabs) {
            projectiles.Add(throwable, 5);
        }
        canThrow = true;
        lineRender = GetComponent<LineRenderer>();
        cc = GetComponent<CharacterController>();

        // Switch();
    }

    private void Switch() {
        int i = 0;
        projectileSelection++;
        if (projectileSelection >= projectiles.Count) projectileSelection = 0;
        foreach (GameObject throwable in projectiles.Keys) {
            if (i++ == projectileSelection) throwableObject = throwable;
        }
    }

    public void AddProjectile(GameObject prefab) {
        projectiles[prefab]++;
    }

    public int GetAmount(GameObject throwable) {
        projectiles.TryGetValue(throwable, out int amount);
        return amount;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            Switch();
        }
        if (Input.GetKeyDown(throwKey))
        {
            projectiles.TryGetValue(throwableObject, out int amount);
            if (amount <= 0) return;
            lineRender.enabled = true;
        }
        else if (Input.GetKeyUp(throwKey) && canThrow)
        {
            lineRender.enabled = false;
            projectiles.TryGetValue(throwableObject, out int amount);
            if (amount <= 0) return;
            Throw();
            
            projectiles[throwableObject]--;
        }
        else if (!Input.GetKey(throwKey))
        {
            lineRender.enabled = false;
        }
        DrawProjection.DrawLine(lineRender, this, camera, transform, collideableLayers);
    }

    private void Throw()
    {
        canThrow = false;

        //This part instantiates the throwable object
        GameObject projectile = Instantiate(throwableObject,throwPoint.position,camera.transform.rotation, EntityManager.Instance.gameObject.transform);

        // Physics
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>(); // Gets the throwable object's rigidbody
        projectileRb.velocity = cc.velocity;


        Vector3 forceDirection = camera.transform.forward;
        RaycastHit hit;

        if(Physics.Raycast(camera.transform.position,camera.transform.forward, out hit, 500f))
        {
            forceDirection = (hit.point - throwPoint.position).normalized; // Calculates the direction from player's camera to raycast hitpoint
        }

        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse); // Impulse so it will only add the force once

        //throwCooldown
        Invoke(nameof(ResetThrow), coolDown);

    }
    
    // Resets the throw
    private void ResetThrow()
    {
        canThrow = true;
    }
}
