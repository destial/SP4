using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public static BulletManager instance;
    // Start is called before the first frame update

    void Start() {
        instance = this;
    }

    public void Shoot(Vector3 position, Vector3 end) {
        GameObject go = Instantiate(bulletPrefab);
        go.gameObject.transform.parent = gameObject.transform.parent;
        Vector3 direction = end - position;
        go.transform.position = position;
        go.transform.rotation = Quaternion.LookRotation(direction);
        go.GetComponent<Bullet>().velocity = direction.normalized;
    } 

    // Update is called once per frame
    void Update()
    {
    }
}
