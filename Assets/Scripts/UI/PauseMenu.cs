using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    Pause _pause = null;

    private void Start()
    {
        _pause = FindObjectOfType<Pause>();
    }

    public void Resume()
    {
       if (_pause != null)
        _pause.ResumeGame();
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
