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
        transform.LookAt(_zombie.Target);
        transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed * 5);

        //Distance between ENEMY & PLAYER
        var distance = Vector3.Distance(transform.position, _zombie.Target.transform.position);

        Debug.Log("CHASING PLAYER");

        //ATTACK, When enemy reached player within distance
        if (distance <= GameSettings.Instance.attackRange)
        {
            Debug.Log("ATTACKED PLAYER");
        }

        //Player is out of range in enemy vision
        else if (distance >= GameSettings.Instance.attackRange)
        {
            return typeof(Patrol);
        }

        return null;
    }
}
