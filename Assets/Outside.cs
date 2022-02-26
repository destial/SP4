using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outside : MonoBehaviour
{
    private Keypad keypad;
    void Start()
    {
        keypad = GetComponent<Keypad>();
    }

    // Update is called once per frame
    void Update()
    {
        if (keypad.doorOpen && LevelManager.instance.currentLevel != 0) LevelManager.instance.currentLevel = 0;
    }
}
