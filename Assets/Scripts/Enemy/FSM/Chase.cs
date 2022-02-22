using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chase : BaseState
{
    //States: Chase, Attack

    private Zombie _zombie;

    const string RUN = "Zombie_Run";
    const string ATTACK = "Zombie_Attack";

    bool attackOutOfRange = true;

    public Chase(Zombie zombie) : base(zombie.gameObject)
    {
        _zombie = zombie;
        animator = _zombie.GetComponentInChildren<Animator>();
        animationManager = _zombie.GetComponent<AnimationManager>();
    }

    public override Type Tick()
    {
        if (_zombie.Target == null)
        {
            return typeof(Patrol);
        }

        else if(_zombie.Target != null)
        {
            //Zombie's Target
            transform.LookAt(_zombie.Target);
            transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed * 2);

            //Distance between ENEMY & PLAYER
            var distance = Vector3.Distance(transform.position, _zombie.Target.transform.position);

            Debug.Log("CHASING PLAYER");

            //ATTACK, When enemy reached player within distance
            if (distance <= GameSettings.Instance.attackRange)
            {
                attackOutOfRange = false;
                Debug.Log("ATTACKED PLAYER");

            }
            //Player is out of range in enemy vision
            else if (distance >= GameSettings.Instance.attackRange)
            {
                attackOutOfRange = true;
                return typeof(Patrol);
            }


            //BOOL for ANIMATIONS
            if(attackOutOfRange == false)
            {
                animationManager.ChangeAnimationState(ATTACK);
            }
            else
            {
                //Callout Animation
                animationManager.ChangeAnimationState(RUN);
            }
        }
        return null;
    }
}
