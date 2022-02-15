using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionEffect;
    public float delay = 3f;

    public float explosionForce = 10f;
    public float radius = 10f;

    public float Damage = 50f;

    private GameObject effect;

    private float distance;
    private void Start()
    {
        Invoke("Explode", delay);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider near in colliders)
        {
            Rigidbody rb = near.GetComponent<Rigidbody>();
            IDamageable damageable = near.GetComponent<IDamageable>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius, 1f, ForceMode.Impulse);
            }
            if(damageable != null)
            {
                
                distance = (transform.position - rb.transform.position).magnitude;
                if(distance <= radius)
                {
                    float damageScale = (1 - (distance / radius)) * Damage; // Calculate new grenade damage based on distance from entity to grenade

                    Debug.Log("Distance :" + distance);
                    damageable?.TakeDamage(damageScale);
                    Debug.Log(damageScale);
                }

            }
            
        }

        //Explosion effect
        effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 1);
        Destroy(gameObject);   
    }
}
