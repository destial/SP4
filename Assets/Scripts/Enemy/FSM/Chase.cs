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

        transform.LookAt(_zombie.Target);
        transform.Translate(Vector3.forward * Time.deltaTime * GameSettings.Instance.zombieSpeed);

        var distance = Vector3.Distance(transform.position, _zombie.Target.transform.position);

        if (distance <= GameSettings.Instance.attackRange)
        {
            return typeof(Attack);
        }

        return null;
    }
}
