using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 velocity;
    public Vector3 travelled;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        travelled += velocity * speed * Time.deltaTime;
        gameObject.transform.position += velocity * speed * Time.deltaTime;
        if (Mathf.Abs(travelled.x) > 50 ||
                Mathf.Abs(travelled.y) > 50 ||
                Mathf.Abs(travelled.z) > 50) {
                Destroy(gameObject);
            }
    }
}
