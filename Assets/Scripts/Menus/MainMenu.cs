using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("DouglasScene");
        GameStateManager.Instance.SetState(GameState.Gameplay);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
