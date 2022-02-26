using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Patrol : BaseState
{
    private Vector3? _destination;
    private float stopDistance = 1.5f;
    private float turnSpeed = 1f;
    private readonly LayerMask _layerMask = LayerMask.NameToLayer("Walls");
    private float _rayDistance = 3.5f;
    private Quaternion _desiredRotation;
    private Vector3 _direction;
    private Zombie _zombie;
    private Vector3 noisePos = Vector3.zero;
    private float timer = 5f;
    private Vector3 lastEnemyPos;
    private float fsmTimer = 0;

    //Animator Vars
    const string WALK = "Zombie_Walk";
    const string IDLE = "Zombie_Idle";

    public Patrol(Zombie zombie):base(zombie.gameObject)
    {
        _zombie = zombie;
        animationManager = _zombie.GetComponent<AnimationManager>();
    }

    public override Type Tick() // Update
    {
        var chaseTarget = checkForAggro();
        fsmTimer += Time.deltaTime;

        if(chaseTarget != null)
        {
            _zombie.setTarget(chaseTarget);
            if (chaseTarget.GetComponentInParent<PlayerMovement>() != null)
            {
                return typeof(Chase);
            }
            else if(chaseTarget.GetComponentInParent<Pipebomb>() != null)
            {
                return typeof(Chase);
            }
            else
            {
                return typeof(Seeking);
            }
        }

        //IDLING AND PATROL MOVEMENT; WHEN ENEMY HAS NO TARGET
        else
        {
            if (fsmTimer <= 10)
            {
                animationManager.ChangeAnimationState(WALK);

                if (!_destination.HasValue || Vector3.Distance(transform.position, _destination.Value) <= stopDistance)
                {
                    findRandomDestination();
                }


                transform.rotation = Quaternion.Slerp(transform.rotation, _desiredRotation, Time.deltaTime * turnSpeed);

                if (isForwardBlocked())
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, _desiredRotation, 0.2f);
                }
                else
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed);
                }

                while (isPathBlocked())
                {
                    findRandomDestination();
                }
            }
            else if (fsmTimer > 10 && fsmTimer <= 15)
            {
                _destination = Vector3.zero;
                animationManager.ChangeAnimationState(IDLE);
            }
            else if (fsmTimer > 15)
            {
                fsmTimer = 0;
            }


            return null;
        }
    }

    private bool isForwardBlocked()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        return Physics.SphereCast(ray, 0.5f, _rayDistance, _layerMask);
    }

    private bool isPathBlocked()
    {
        Ray ray = new Ray(transform.position, _direction);
        return Physics.SphereCast(ray, 0.5f, _rayDistance, _layerMask);
    }

    private void findRandomDestination()
    {
        Vector3 testPosition = (transform.position + transform.forward * 4f)
            + new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0, UnityEngine.Random.Range(-4.5f, 4.5f));

        _destination = new Vector3(testPosition.x, transform.position.y, testPosition.z);

        _direction = Vector3.Normalize(_destination.Value - transform.position);
        _direction = new Vector3(_direction.x, 0f, _direction.z);
        _desiredRotation = Quaternion.LookRotation(_direction);
    }

    Quaternion startingAngle = Quaternion.AngleAxis(-40, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(80 / 5, Vector3.up);

    private Transform checkForAggro()
    {
        
        float aggroRadius = 40f;
        
        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * transform.forward;
        var pos = transform.position;
        pos.y += 1;

        Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRadius); // Stores all colliders that are within enemy's radius

        // Check for objects
        foreach (Collider near in colliders)
        {
            if(near.gameObject.GetComponent<Pipebomb>() != null)
            {
                return near.gameObject.transform;
            }
            else if(near.gameObject.GetComponent<Stone>() != null && near.gameObject.GetComponent<Stone>().IsGrounded())
            {
                if (Vector3.Distance(near.gameObject.transform.position, transform.position) <= aggroRadius)
                {
                    near.gameObject.GetComponent<Stone>().enabled = false;
                    return near.gameObject.transform;
                }
                    
                continue;
            }
        }

        // Field of view
        for (var i = 0; i < 5; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                if (hit.collider != null)
                {
                    var drone = hit.collider.GetComponentInParent<PlayerMovement>();
                    if (drone != null)
                    {
                        return drone.transform;
                    }
                }
            }
            direction = stepAngle * direction;
        }

        return null;
    }
}
