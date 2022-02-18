using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    [HideInInspector] public Vector3 velocity;
    private Vector3 travelled;
    private Rigidbody rb;
    [HideInInspector] public float damage;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = velocity * speed;
    }

    // Update is called once per frame
    void Update()
    {
        travelled += rb.velocity * Time.deltaTime;
        if (Mathf.Abs(travelled.x) > 50 ||
            Mathf.Abs(travelled.y) > 50 ||
            Mathf.Abs(travelled.z) > 50) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        IDamageable damageable = collision.transform.GetComponent<IDamageable>();
        if (damageable == null) { 
            collision.transform.GetComponentInChildren<IDamageable>();
        }
        if (damageable == null) { 
            collision.transform.GetComponentInParent<IDamageable>();
        }
        damageable?.TakeDamage(damage);
        Destroy(gameObject);
    }
}
