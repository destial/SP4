using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Idle : BaseState
{
    private Zombie _zombie;

    public Idle(Zombie zombie) : base(zombie.gameObject)
    {
        _zombie = zombie;
        animator = _zombie.GetComponentInChildren<Animator>();
    }

    public override Type Tick()
    {

        float fsmTimer = 0;
        fsmTimer += Time.deltaTime;

        //Check if player is within vision
        var chaseTarget = checkForAggro();
        if (chaseTarget != null)
        {
            Debug.Log("Chasing Player");
            _zombie.setTarget(chaseTarget);
            return typeof(Chase);
        }



        return null;
    }

    //Checking For Player around environment
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
