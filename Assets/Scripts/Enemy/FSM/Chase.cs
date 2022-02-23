using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chase : BaseState
{
    private Zombie _zombie;

    public Chase(Zombie zombie) : base(zombie.gameObject)
    {
        _zombie = zombie;
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
        var distance = Vector3.Distance(transform.position, _zombie.Target.transform.position);

        Debug.Log("CHASING PLAYER");

        //ATTACK, When enemy reached player within distance
        if (distance <= GameSettings.Instance.attackRange)
        {
            Debug.Log("ATTACKED PLAYER");
        }

        //Player is out of range in enemy vision
        else
        {
            //transform.LookAt(targetPos);
            transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed * 5);
            return typeof(Patrol);
        }

        return null;
    }
}
