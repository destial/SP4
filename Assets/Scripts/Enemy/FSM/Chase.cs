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
    private float damageCooldown = 0f;
    public Chase(Zombie zombie) : base(zombie.gameObject)
    {
        _zombie = zombie;
        animationManager = _zombie.GetComponent<AnimationManager>();
    }

    private bool isForwardBlocked()
    {
        Ray ray = new Ray(transform.position, _zombie.Target.position - transform.position);
        return Physics.SphereCast(ray, 0.5f, _rayDistance, _layerMask);
    }

    public override Type Tick()
    {
        if (_zombie.Target == null) return typeof(Patrol);

        //Zombie's Target
        Vector3 targetPos = new Vector3(_zombie.Target.position.x,
                                        transform.position.y,
                                        _zombie.Target.position.z);
        
        transform.LookAt(targetPos);
        Vector3 direction = (targetPos - transform.position).normalized;
        Vector3 velocity = direction * GameSettings.Instance.zombieSpeed * 3;
        _zombie.GetComponent<Rigidbody>().velocity = velocity;

        //Distance between ENEMY & PLAYER
        var distance = Vector3.Distance(transform.position, targetPos);

        if(distance <= GameSettings.Instance.aggroRadius)
        {
            animationManager.ChangeAnimationState(ATTACK);
            if(_zombie.Target.GetComponent<PlayerManager>() != null)
            {
                if (damageCooldown <= 0)
                {
                    PlayerDamage(5);
                    damageCooldown = 2f;
                }
                else
                {
                    damageCooldown -= Time.deltaTime;
                }
            }
        }
        else if(distance <= 20f)
        {
            if (!isForwardBlocked())
            {
                animationManager.ChangeAnimationState(RUN);
            }
        }

        //Player is out of range in enemy vision
        else
        {
            return typeof(Patrol);
        }




        return null;
    }

    private void PlayerDamage(float damage)
    {
        _zombie.Target.GetComponent<IDamageable>().TakeDamage(damage);
    }
}


