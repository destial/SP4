using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                Cursor.visible = false;
                PlayerMovement.instance.canMove = true;
            }
            else
            {
                Pause();
                Cursor.visible = true;
                PlayerMovement.instance.canMove = false;
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameStateManager.Instance.SetState(GameState.Gameplay);
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Paused);
        GameIsPaused = true;
    }
}
