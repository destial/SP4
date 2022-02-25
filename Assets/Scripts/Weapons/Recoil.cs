using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    public float recoilX = -2f;
    public float recoilY = 2f;
    public float recoilZ = 0.35f;

    public float snappiness = 6f;
    public float returnSpeed = 2f;

    void Start()
    {
        
    }

    void Update()
    {
        //if (Vector3.Dot(targetRotation, Vector3.zero) >= 0.99f) targetRotation = Vector3.zero;
        //else 
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        //if (Vector3.Dot(currentRotation, targetRotation) >= 0.99f) currentRotation = targetRotation;
        //else 
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire(WeaponMeta weapon) {
        float recX = weapon.isScoping ? -1 : recoilX;
        float recY = weapon.isScoping ? 0 : recoilY;
        float recZ = weapon.isScoping ? 1 : recoilZ;
        targetRotation += new Vector3(recX, Random.Range(-recY, recY), Random.Range(-recZ, recZ));
    }
}
