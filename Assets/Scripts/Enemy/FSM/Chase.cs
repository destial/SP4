using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chase : BaseState
{
    private Zombie _zombie;

    //Animation Vars
    const string RUN = "Zombie_Run";
    const string ATTACK = "Zombie_Attack";


    public Chase(Zombie zombie) : base(zombie.gameObject)
    {
        _zombie = zombie;
        animationManager = _zombie.GetComponent<AnimationManager>();
    }

    public override Type Tick()
    {
        if (_zombie.Target == null) return typeof(Patrol);
        //Zombie's Target
        Vector3 targetPos = new Vector3(_zombie.Target.position.x,
                                        transform.position.y,
                                        _zombie.Target.position.z);
        //transform.LookAt(_zombie.Target);
        transform.LookAt(targetPos);

        //Distance between ENEMY & PLAYER
        var distance = Vector3.Distance(transform.position, targetPos);

        // Debug.Log("CHASING PLAYER");
        //transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed * 5);
        //ATTACK, When enemy reached player within distance
        if(distance <= GameSettings.Instance.attackRange)
        {
            animationManager.ChangeAnimationState(ATTACK);
            Debug.Log("ATTACKED PLAYER");
        }
        else if(distance <= 40f) //GameSettings.Instance.aggroRadius)
        {
            Debug.Log("CHASING TARGET");
            transform.Translate(transform.forward.normalized * Time.deltaTime * GameSettings.Instance.zombieSpeed * 5);
            animationManager.ChangeAnimationState(RUN);
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
