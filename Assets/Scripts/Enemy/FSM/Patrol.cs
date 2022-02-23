using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Patrol : BaseState
{

    private Vector3? _destination;
    private float stopDistance = 1f;
    private float turnSpeed = 1f;
    private readonly LayerMask _layerMask = LayerMask.NameToLayer("Walls");
    private float _rayDistance = 3.5f;
    private Quaternion _desiredRotation;
    private Vector3 _direction;
    private Zombie _zombie;
    private Vector3 noisePos = Vector3.zero;
    private float timer = 5f;
    private Vector3 lastEnemyPos;
    

    //Animator Vars
    const string WALK = "Zombie_Walk";

    //void Start()
    //{
    //    GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    //}

    //private void OnDestroy()
    //{
    //    GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    //}

    //private void OnGameStateChanged(GameState newGameState)
    //{
    //    enabled = newGameState == GameState.Gameplay;
    //}

    public Patrol(Zombie zombie):base(zombie.gameObject)
    {
        _zombie = zombie;
        animationManager = _zombie.GetComponent<AnimationManager>();
    }

    public override Type Tick() // Update
    {
        Debug.Log("Patroling!");
        var chaseTarget = checkForAggro();
        if(chaseTarget != null)
        {
            Debug.Log("SWITCH TO CHASE STATE!");
            _zombie.setTarget(chaseTarget);
            if (chaseTarget.GetComponentInParent<PlayerMovement>() != null)
            {
                Debug.Log("Chasing!");
                
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
        

        else
        {
            animationManager.ChangeAnimationState(WALK);
        }

       if(_destination.HasValue == false || Vector3.Distance(transform.position,_destination.Value) <= stopDistance)
        {
            findRandomDestination();
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, _desiredRotation, Time.deltaTime * turnSpeed);

        if(isForwardBlocked())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _desiredRotation, 0.2f);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed);
        }

        Debug.DrawRay(transform.position, _direction * _rayDistance, Color.red);
        while(isPathBlocked())
        {
            findRandomDestination();
            Debug.Log("WALL!");
        }

        return null;

    }

    private bool isForwardBlocked()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        return Physics.SphereCast(ray, 0.5f, _rayDistance, _layerMask);
    }

    private bool isPathBlocked()
    {
        Ray ray = new Ray(transform.position, _direction);
        return Physics.SphereCast(ray, _rayDistance, _layerMask);
    }

    private void findRandomDestination()
    {
        Vector3 testPosition = (transform.position + (transform.forward * 4f)) 
            + new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0, UnityEngine.Random.Range(-4.5f, 4.5f));

        _destination = new Vector3(testPosition.x, 0f, testPosition.z);

        _direction = Vector3.Normalize(_destination.Value - transform.position);
        _direction = new Vector3(_direction.x, 0f, _direction.z);
        _desiredRotation = Quaternion.LookRotation(_direction);
        Debug.Log("Found Random Direction");
    }

    private void CheckNoise(Vector3 target)
    {
        lastEnemyPos = transform.position;
        _direction = Vector3.Normalize(target - transform.position);
        _direction = new Vector3(_direction.x, 0f, _direction.z); // so it does not rotate on the y-axis
        Vector3 newPos = transform.position +  _direction;
        _destination = new Vector3(newPos.x, 0f, newPos.z);
        _desiredRotation = Quaternion.LookRotation(_direction);
        
        Debug.Log("checking noise");
    }

    private Transform checkForAggro()
    {
        Quaternion startingAngle = Quaternion.AngleAxis(-40, Vector3.up);
        Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

        float aggroRadius = 40f;
        
        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
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
        //foreach(Stone stone in EntityManager.Instance.GetComponentsInChildren<Stone>())
        //{
        //    if(stone.isActiveAndEnabled && stone.IsGrounded())
        //    {
        //        if (Vector3.Distance(stone.getLastPos(), transform.position) <= aggroRadius)
        //        {
        //            CheckNoise(stone.getLastPos());
        //            stone.enabled = false;
        //        }
        //    }
        //}


        // Field of view
        for (var i = 0; i < 24; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider);
                    var drone = hit.collider.GetComponentInParent<PlayerMovement>();
                    if (drone != null)
                    {
                        //When zombie hit player
                        Debug.Log("Player Found");
                        Debug.DrawLine(pos, direction * hit.distance, Color.red);
                        return drone.transform;
                    }
                }
                else
                {
                    Debug.DrawLine(pos, direction * hit.distance, Color.yellow);
                }
            }
            else
            {
                //Debug.Log("Player Not Found");
                Debug.DrawLine(pos, direction * aggroRadius, Color.white);
            }
            direction = stepAngle * direction;
        }

        return null;
    }
}
