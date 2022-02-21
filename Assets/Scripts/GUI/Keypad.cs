using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    public string curPassword = "12345";
    public string input;
    public bool onTrigger;
    public bool doorOpen;
    public bool keypadScreen;
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
        if (input == curPassword)
        {
            doorOpen = true;
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
                GUI.Box(new Rect(400, 250, 200, 25), "Press 'E' to open keypad");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    keypadScreen = true;
                    onTrigger = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    PlayerMovement.instance.canMove = false;
                }
            }

            if (keypadScreen)
            {
                GUI.Box(new Rect(300, 50, 320, 455), "");
                GUI.Box(new Rect(305, 55, 310, 25), input);

                if (GUI.Button(new Rect(305, 85, 100, 100), "1"))
                {
                    input = input + "1";
                }

                if (GUI.Button(new Rect(410, 85, 100, 100), "2"))
                {
                    input = input + "2";
                }

                if (GUI.Button(new Rect(515, 85, 100, 100), "3"))
                {
                    input = input + "3";
                }

                if (GUI.Button(new Rect(305, 190, 100, 100), "4"))
                {
                    input = input + "4";
                }

                if (GUI.Button(new Rect(410, 190, 100, 100), "5"))
                {
                    input = input + "5";
                }

                if (GUI.Button(new Rect(515, 190, 100, 100), "6"))
                {
                    input = input + "6";
                }

                if (GUI.Button(new Rect(305, 295, 100, 100), "7"))
                {
                    input = input + "7";
                }

                if (GUI.Button(new Rect(410, 295, 100, 100), "8"))
                {
                    input = input + "8";
                }

                if (GUI.Button(new Rect(515, 295, 100, 100), "9"))
                {
                    input = input + "9";
                }

                if (GUI.Button(new Rect(305, 400, 100, 100), "CLEAR"))
                {
                    input = "";
                }

                if (GUI.Button(new Rect(410, 400, 100, 100), "0"))
                {
                    input = input + "0";
                }

                if (GUI.Button(new Rect(515, 400, 100, 100), "QUIT"))
                {
                    input = "";
                    keypadScreen = false;
                    onTrigger = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    PlayerMovement.instance.canMove = true;
                }
            }
        }
    }
}
