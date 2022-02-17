using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, BaseState> _availableStates;

    public BaseState currentState { get; private set; }
    public event Action<BaseState> OnStateChanged;

    //Dictionary States  
    public void setState(Dictionary<Type, BaseState> states)
    {
        _availableStates = states;
        currentState = states.Values.GetEnumerator().Current;
    }

    // Update is called once per frame
    void Update()
    {
        if (_availableStates == null) return;
        //Just to make sure that current state != NULL
        if(currentState == null)
        {
            var iterator = _availableStates.Values.GetEnumerator();
            while (currentState == null)
            {
                currentState = iterator.Current;
                iterator.MoveNext();
            }
        }

        if (currentState == null) return;
        var nextState = currentState?.Tick();

        if(nextState != null && nextState != currentState?.GetType())
        {
            switchToNewState(nextState);
        }
    }

    private void switchToNewState(Type nextState)
    {
        currentState = _availableStates[nextState];
        OnStateChanged?.Invoke(currentState);
    }
}
