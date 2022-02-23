using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public float noiseRadius = 40f;
    private Vector3 lastHitpos = Vector3.zero;
    private void Start()
    {
    }

    private void Update()
    {
        //if(Physics.CheckSphere(transform.position,noiseRadius,0,QueryTriggerInteraction.Collide))
        //{
        //    Debug.Log("HIT");
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        //foreach(Zombie zombie in EntityManager.Instance.GetComponentsInChildren<Zombie>())
        //{
        //    zombie.gameObject.transform.LookAt(transform.position);
        //}
        //lastHitpos = transform.position;
    }

    public Vector3 getLastPos()
    {
        return lastHitpos;
    }


}
