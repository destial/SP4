using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, BaseState> _availableStates;

    public BaseState currentState { get; private set; }
    public event Action<BaseState> OnStateChanged;

    private bool isDead = false;

    const string DEATH = "Zombie_Death";

    //Dictionary States  
    public void setState(Dictionary<Type, BaseState> states)
    {
        _availableStates = states;
        currentState = states.Values.GetEnumerator().Current;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.CurrentGameState == GameState.Paused) return;
        if (_availableStates == null) return;

        GetComponent<Animator>().enabled = GameStateManager.Instance.CurrentGameState != GameState.Paused;

        if (GetComponent<IDamageable>().GetHP() <= 0 && isDead == false)
        {
            isDead = true;
            GetComponent<AnimationManager>().ChangeAnimationState(DEATH);
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Destroy(gameObject, 2);
            return;
        }
        if (isDead) return;


        //Just to make sure that current state != NULL
        if (currentState == null)
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
