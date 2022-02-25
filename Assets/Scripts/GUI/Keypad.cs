using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    public string currentPassword = "12345";
    [HideInInspector] public string input;
    [HideInInspector] public bool onTrigger;
    [HideInInspector] public bool doorOpen;
    [HideInInspector] public bool keypadScreen;
    public Transform doorHinge;

    void OnTriggerEnter(Collider other)
    {
        onTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        onTrigger = false;
        keypadScreen = false;
        input = "";
    }

    void Update()
    {
        if (input == currentPassword)
        {
            doorOpen = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerMovement.instance.canMove = true;
            GameStateManager.Instance.SetState(GameState.Gameplay);
        }

        if (doorOpen)
        {
            var newRot = Quaternion.RotateTowards(doorHinge.rotation, Quaternion.Euler(0.0f, -90.0f, 0.0f), Time.deltaTime * 250);
            doorHinge.rotation = newRot;
        }
    }

    void OnGUI()
    {
        if (!doorOpen)
        {
            if (onTrigger)
            {
                // Rect rect = PlayerManager.instance.GetComponentInChildren<Camera>().
                GUI.Box(new Rect(850, 450, 200, 25), "Press 'F' to open keypad");

                if (Input.GetKeyDown(KeyCode.F))
                {
                    keypadScreen = true;
                    onTrigger = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    PlayerMovement.instance.canMove = false;
                    GameStateManager.Instance.SetState(GameState.Keypad);
                }
            }

            if (keypadScreen)
            {
                GUI.Box(new Rect(780, 250, 320, 455), "");
                GUI.Box(new Rect(785, 255, 310, 25), input);

                if (GUI.Button(new Rect(785, 285, 100, 100), "1"))
                {
                    input = input + "1";
                }

                if (GUI.Button(new Rect(890, 285, 100, 100), "2"))
                {
                    input = input + "2";
                }

                if (GUI.Button(new Rect(995, 285, 100, 100), "3"))
                {
                    input = input + "3";
                }

                if (GUI.Button(new Rect(785, 390, 100, 100), "4"))
                {
                    input = input + "4";
                }

                if (GUI.Button(new Rect(890, 390, 100, 100), "5"))
                {
                    input = input + "5";
                }

                if (GUI.Button(new Rect(995, 390, 100, 100), "6"))
                {
                    input = input + "6";
                }

                if (GUI.Button(new Rect(785, 495, 100, 100), "7"))
                {
                    input = input + "7";
                }

                if (GUI.Button(new Rect(890, 495, 100, 100), "8"))
                {
                    input = input + "8";
                }

                if (GUI.Button(new Rect(995, 495, 100, 100), "9"))
                {
                    input = input + "9";
                }

                if (GUI.Button(new Rect(785, 600, 100, 100), "CLEAR"))
                {
                    input = "";
                }

                if (GUI.Button(new Rect(890, 600, 100, 100), "0"))
                {
                    input = input + "0";
                }

                if (GUI.Button(new Rect(995, 600, 100, 100), "QUIT"))
                {
                    input = "";
                    keypadScreen = false;
                    onTrigger = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    PlayerMovement.instance.canMove = true;
                    GameStateManager.Instance.SetState(GameState.Gameplay);
                }
            }
        }
    }
}
