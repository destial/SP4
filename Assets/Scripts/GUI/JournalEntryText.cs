using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalEntryText : MonoBehaviour
{
    [SerializeField] public string journalEntryNo;
    
    [HideInInspector] public bool onTrigger;
    [HideInInspector] public bool textScreen;

    public GameObject journalEntryUI;

    void OnTriggerEnter(Collider other)
    {
        onTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        onTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (onTrigger)
        {
            // Rect rect = PlayerManager.instance.GetComponentInChildren<Camera>().
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 25), "Press 'F' to open Journal Entry #" + journalEntryNo);

            if (Input.GetKeyDown(KeyCode.F))
            {
                textScreen = true;
                onTrigger = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PlayerMovement.instance.canMove = false;
                GameStateManager.Instance.SetState(GameState.Keypad);
            }
        }

        if (textScreen)
        {
            JournalEntries();
        }
    }

    public void JournalEntries()
    {
        journalEntryUI.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Keypad);
        Cursor.visible = true;
        PlayerMovement.instance.canMove = false;
    }

    public void QuitText()
    {
        textScreen = false;
        GameStateManager.Instance.SetState(GameState.Gameplay);
        Cursor.visible = false;
        PlayerMovement.instance.canMove = true;
        onTrigger = true;
    }
}
