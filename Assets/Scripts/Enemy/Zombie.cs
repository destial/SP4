using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Zombie : MonoBehaviour
{
    //private NavMeshAgent agent = null;
    //[SerializeField] private Transform target;

    [SerializeField] public Team _team;

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
            { typeof(Chase), new Chase(this)},
            { typeof(Attack), new Attack(this)}

        };

        GetComponent<StateMachine>().setState(states);
    }


    public void setTarget(Transform target)
    {
        Target = target;
    }
}


public enum Team
{
    Red,
    Blue
}
