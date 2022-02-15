using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Attack : BaseState
{
    private Zombie _zombie;
    private float _attackReadyTimer;

    public Attack(Zombie zombie) : base(zombie.gameObject)
    {
        _zombie = zombie;
    }

    public override Type Tick()
    {
        if (_zombie.Target == null)
        {
            _attackReadyTimer = 1f;
            return typeof(Patrol);
        }

        _attackReadyTimer -= Time.deltaTime;

        if(_attackReadyTimer <= 0f)
        {
            Debug.Log("Attack!");
        }

        return null;
    }
}
