using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameStateManager.Instance.SetState(GameState.Gameplay);
        GameIsPaused = false;
        Cursor.visible = false;
        PlayerMovement.instance.canMove = true;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Paused);
        GameIsPaused = true;
        Cursor.visible = true;
        PlayerMovement.instance.canMove = false;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        //GameIsPaused = false;
    }

    public void QuitGame()
    {
        //Debug.Log("Quiting Game");
        Application.Quit();
    }
}
