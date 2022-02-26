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
    const string RUN = "Zombie_Run";
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
        if (_zombie.Target == null) return typeof(Patrol);
        var target = checkForAggro();
        if (target != null)
        {
            if (target.transform.GetComponent<PlayerMovement>() != null)
            {
                _zombie.setTarget(target);
                animationManager.ChangeAnimationState(RUN);
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
                    if(timer <= 0f)
                    {

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
                    transform.LookAt(targetPos);
                    Vector3 velocity = (targetPos - transform.position).normalized * GameSettings.Instance.zombieSpeed * 2;
                    _zombie.GetComponent<Rigidbody>().velocity = velocity;
                }
            }
        }
        return null;
    }

    private Transform checkForAggro()
    {
        Quaternion startingAngle = Quaternion.AngleAxis(-40, Vector3.up);
        Quaternion stepAngle = Quaternion.AngleAxis(80 / 5, Vector3.up);

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
        for (var i = 0; i < 5; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                if (hit.collider != null)
                {
                    //Debug.Log(hit.collider);
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
