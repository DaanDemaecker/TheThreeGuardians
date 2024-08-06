using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField]
    GameObject _pauseMenu = null;

    const string PAUSE = "Pause";

    bool _paused = false;

    private void Start()
    {
        if(_pauseMenu != null)
        {
            _pauseMenu.SetActive(false);
        }
    }
    void Update()
    {
        if(Input.GetButtonDown(PAUSE))
        {
            if(_paused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        _paused = true;
        Time.timeScale = 0.0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        AudioListener.pause = true;

        if (_pauseMenu != null)
        {
            _pauseMenu.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        _paused = false;
        Time.timeScale = 1.0f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        AudioListener.pause = false;

        if (_pauseMenu != null)
        {
            _pauseMenu.SetActive(false);
        }
    }

}
