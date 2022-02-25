using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Seeking : BaseState
{
    private Zombie _zombie;
    private Vector3 lastEnemyPos;
    private Quaternion right;
    private Quaternion left;
    private float timer = 5f;
    Quaternion startingAngle = Quaternion.AngleAxis(-40, Vector3.up);
    Quaternion endingAngle = Quaternion.AngleAxis(40, Vector3.up);

    private bool lookingLeft = true;

    const string IDLE = "Zombie_Idle";
    private void Start()
    {
    }
    public Seeking(Zombie zombie) : base(zombie.gameObject)
    {
        _zombie = zombie;
        animationManager = _zombie.GetComponent<AnimationManager>();
    }

    public override Type Tick()
    {
        Debug.Log("Seeking!!!!");
        if (_zombie.Target == null) return typeof(Patrol);
        var target = checkForAggro();
        if (target != null)
        {
            if (target.transform.GetComponent<PlayerMovement>() != null)
            {
                _zombie.setTarget(target);
                return typeof(Chase);
            }
        }
        //Zombie's Target

        //transform.LookAt(_zombie.Target);
        Vector3 targetPos = new Vector3(_zombie.Target.position.x,
                                transform.position.y,
                                _zombie.Target.position.z);

        //Distance between ENEMY & PLAYER
        var distance = Vector3.Distance(transform.position, _zombie.Target.transform.position);
        //Seeking, When enemy reached player within distance
        if (distance <= 40f)
        {
            if (_zombie.Target.gameObject.GetComponent<Stone>() != null)
            {
                if(distance <= 4f)
                {
                    //Debug.Log("Stopping");
                    //transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);//make variable 
                    //if (right == null)
                    //{
                    //    right = transform.rotation * startingAngle;
                    //}
                    //if (left == null)
                    //{
                    //    left = transform.rotation * endingAngle;
                    //}
                    //Vector3 direction;
                    //if (lookingLeft)
                    //{
                    //    direction = left * Vector3.forward;
                    //}
                    //else
                    //{
                    //    Debug.Log("Running here");
                    //    direction = right * Vector3.forward;
                    //}
                    //Quaternion qDirection = Quaternion.LookRotation(direction);
                    //transform.RotateAround(transform.position, transform.up, Time.deltaTime * 60 * (!lookingLeft ? 1 : -1));
                    ////transform.rotation = Quaternion.Lerp(transform.rotation, qDirection, 0.1f * dir);
                    ////if (Equals(qDirection, transform.rotation)) {
                    //if (Quaternion.Angle(qDirection, transform.rotation) <= 2f)
                    //{
                    //    Debug.Log("Searching! Left Right!");
                    //    lookingLeft = !lookingLeft;

                    //    if (lookingLeft)
                    //    {
                    //        return typeof(Patrol);
                    //    }
                    //}
                    
                    if(timer <= 0f)
                    {
                        Debug.Log("Returning to patrol state");
                        timer = 5f;
                        _zombie.Target.gameObject.SetActive(false);
                        return typeof(Patrol);
                    }
                    else
                    {
                        timer -= Time.deltaTime;
                    }
                    checkForAggro();
                    animationManager.ChangeAnimationState(IDLE);
                }
                else
                {
                    Debug.Log("Running (Curious)");
                    transform.LookAt(targetPos);
                    transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed * 5);
                }
            }
        }
        //Player is out of range in enemy vision
        //else
        //{
        //    //transform.LookAt(targetPos);

        //    return typeof(Patrol);
        //}
        return null;
    }


    private bool TurnLeft()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        left = transform.rotation * endingAngle;
        Vector3 direction = left * Vector3.forward;
        Quaternion qDirection = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, qDirection, Time.deltaTime * 0.5f);
        return Quaternion.Angle(qDirection, transform.rotation) <= 0.01f;
    }

    private bool TurnRight()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        right = transform.rotation * startingAngle;
        Vector3 direction = right * Vector3.forward;
        Quaternion qDirection = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, qDirection, Time.deltaTime * 0.5f);
        return Quaternion.Angle(qDirection, transform.rotation) <= 0.01f;
    }

    private void CheckNoise(Vector3 target)
    {
        lastEnemyPos = transform.position;

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
            if (near.gameObject.GetComponent<Pipebomb>() != null)
            {
                return near.gameObject.transform;
            }
            else if (near.gameObject.GetComponent<Stone>() != null && near.gameObject.GetComponent<Stone>().IsGrounded())
            {
                if (Vector3.Distance(near.gameObject.transform.position, transform.position) <= aggroRadius)
                    return near.gameObject.transform;
                continue;
            }
        }

        // Field of view
        for (var i = 0; i < 24; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                if (hit.collider != null)
                {
                    //Debug.Log(hit.collider);
                    var drone = hit.collider.GetComponentInParent<PlayerMovement>();
                    if (drone != null)
                    {
                        //When zombie hit player
                        //Debug.Log("Player Found");
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
