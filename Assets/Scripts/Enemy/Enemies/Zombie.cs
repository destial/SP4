using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Zombie : MonoBehaviour
{

    public Transform Target { get; private set; }

    public StateMachine StateMachine => GetComponent<StateMachine>();

    void Start()
    {
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            { typeof(Patrol), new Patrol(this)},
            { typeof(Chase), new Chase(this)}

        };

        GetComponent<StateMachine>().setState(states);
    }


    public void setTarget(Transform target)
    {
        Target = target;
    }
}
