using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipebomb : MonoBehaviour
{
    public GameObject explosionEffect;
    public float delay = 5f;

    public float explosionForce = 10f;
    public float radius = 10f;

    public float damage = 100f;

    private float distance;

    private bool exploded = false;
    private void Start()
    {
        Invoke("Explode", delay);
    }

    public void Explode()
    {
        if (exploded) return;
        exploded = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider near in colliders)
        {
            Rigidbody rb = near.GetComponent<Rigidbody>();
            IDamageable damageable = near.GetComponent<IDamageable>();
            RaycastHit Hit;
            if(Physics.Raycast(transform.position,near.gameObject.transform.position - transform.position,out Hit,radius))
            {
                if(Hit.collider == near)
                {
                    Debug.Log("Raycast Hit: " + Hit.collider.gameObject.name);
                    if (rb != null)
                    {
                        rb.AddExplosionForce(explosionForce, transform.position, radius, 1f, ForceMode.Impulse);
                    }
                    if (damageable != null)
                    {

                        distance = (transform.position - rb.transform.position).magnitude;
                        if (distance <= radius)
                        {
                            float damageScale = (1 - (distance / radius)) * damage; // Calculate new grenade damage based on distance from entity to grenade

                            Debug.Log("Distance :" + distance);
                            damageable?.TakeDamage(damageScale);
                            Debug.Log(damageScale);
                        }
                    }
                }
            }
        }

        //Explosion effect
        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 1);
        Destroy(gameObject);
    }
}
