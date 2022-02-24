using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Zombie : MonoBehaviour
{

    public Transform Target { get; private set; }

    public StateMachine StateMachine => GetComponent<StateMachine>();

    private void Start()
    {
        InitializeStateMachine();
    }

    public void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            { typeof(Patrol), new Patrol(this)},
            { typeof(Seeking), new Seeking(this) },
            { typeof(Chase), new Chase(this)}
        };

        GetComponent<StateMachine>().setState(states);
    }

    private void Update()
    {
        GetComponent<Animator>().enabled = GameStateManager.Instance.CurrentGameState != GameState.Paused;
    }


    public void setTarget(Transform target)
    {
        Target = target;
    }
}
