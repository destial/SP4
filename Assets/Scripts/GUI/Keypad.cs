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
        int startX = (Screen.width / 2 - 160);
        int startY = (Screen.height / 2 - 227);
        if (!doorOpen)
        {
            if (onTrigger)
            {
                // Rect rect = PlayerManager.instance.GetComponentInChildren<Camera>().
                GUI.Box(new Rect(Screen.width / 2 - 100, Screen.width / 2 - 12.5f, 200, 25), "Press 'F' to open keypad");

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
                GUI.Box(new Rect(startX, startY, 320, 455), "");
                GUI.Box(new Rect(startX + 5, startY + 5, 310, 25), input);

                int endY = 0;
                int i = 1;
                for (int y = 0; y < 3; y++) {
                    for (int x = 0; x < 3; x++) {
                        if (GUI.Button(new Rect(startX + 5 + (x * 105), startY + 30 + (y * 105), 100, 100), "" + i)) {
                            input += "" + i;
                        }
                        i++;
                    }
                    endY = startY + 30 + (y * 105);
                }

                if (GUI.Button(new Rect(startX + 5, endY + 105, 100, 100), "CLEAR"))
                {
                    input = "";
                }

                if (GUI.Button(new Rect(startX + 5 + 105, endY + 105, 100, 100), "0"))
                {
                    input = input + "0";
                }

                if (GUI.Button(new Rect(startX + 5 + 210, endY + 105, 100, 100), "QUIT"))
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
