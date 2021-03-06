using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;
    
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GameStateManager.Instance.CurrentGameState != GameState.Gameplay) return;
        if(Input.GetMouseButton(0))
        {
            shootInput?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            reloadInput?.Invoke();
        }
    }
}
