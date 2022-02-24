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
        lastHitpos = transform.position;
    }

    public Vector3 getLastPos()
    {
        return lastHitpos;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 0.1f);
    }


}
