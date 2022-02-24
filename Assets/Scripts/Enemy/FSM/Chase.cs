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
        transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed * 5);

        //Distance between ENEMY & PLAYER
        var distance = Vector3.Distance(transform.position, _zombie.Target.transform.position);

        //ATTACK, When enemy reached player within distance
        if (distance <= GameSettings.Instance.aggroRadius)
        {
            animationManager.ChangeAnimationState(ATTACK);
            Debug.Log("ATTACKED PLAYER");
        }

        else
        {
            Debug.Log("CHASING PLAYER");
            animationManager.ChangeAnimationState(RUN);
        }

        return null;
    }
}
