using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : BaseState
{
    private Zombie _zombie;

    //Animation Vars
    const string RUN = "Zombie_Run";
    const string ATTACK = "Zombie_Attack";

    public Death(Zombie zombie) : base(zombie.gameObject)
    {
        _zombie = zombie;
        animationManager = _zombie.GetComponent<AnimationManager>();
    }

    public override Type Tick()
    {
        return null;
    }
}
