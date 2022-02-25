using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chase : BaseState
{
    private Zombie _zombie;

    private readonly LayerMask _layerMask = LayerMask.NameToLayer("Walls");
    private float _rayDistance = 3.5f;
    private Quaternion _desiredRotation;

    //Animation Vars
    const string RUN = "Zombie_Run";
    const string ATTACK = "Zombie_Attack";




    public Chase(Zombie zombie) : base(zombie.gameObject)
    {
        _zombie = zombie;
        animationManager = _zombie.GetComponent<AnimationManager>();
    }

    private bool isForwardBlocked()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        return Physics.SphereCast(ray, 0.5f, _rayDistance, _layerMask);
    }

    public override Type Tick()
    {
        if (_zombie.Target == null) return typeof(Patrol);


        //Zombie's Target
        Vector3 targetPos = new Vector3(_zombie.Target.position.x,
                                        transform.position.y,
                                        _zombie.Target.position.z);

        transform.LookAt(_zombie.Target);
        
        //transform.LookAt(targetPos);
        transform.Translate(transform.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed * 2);

        //Distance between ENEMY & PLAYER
        var distance = Vector3.Distance(transform.position, targetPos);

        if(distance <= GameSettings.Instance.aggroRadius)
        {
            animationManager.ChangeAnimationState(ATTACK);
            Debug.Log("ATTACKED PLAYER");
        }
        else if(distance <= 20f)
        {
            if (isForwardBlocked())
            {
                Debug.Log("FORWARD BLOCKED TRUE! CHASE");
                transform.rotation = Quaternion.Lerp(transform.rotation, _desiredRotation, 0.2f);
            }
            else
            {
                Debug.Log("FORWARD BLOCKED FALSE! CHASE");
                //transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed * 5);

                Debug.Log("CHASING TARGET");
                transform.Translate(transform.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed * 5);
                animationManager.ChangeAnimationState(RUN);
            }
        }
        //Player is out of range in enemy vision
        else
        {
            //transform.LookAt(targetPos);
            return typeof(Patrol);
        }




        return null;
    }
}


