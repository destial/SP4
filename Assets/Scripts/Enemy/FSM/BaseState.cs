using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseState
{
  public BaseState(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
    }

    protected GameObject gameObject;
    protected Transform transform;
    public Animator animator;

    public AnimationManager animationManager { get; set; }

    public abstract Type Tick();
}
