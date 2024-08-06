using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainMenu = null;
    [SerializeField]
    private GameObject _customFight = null;

    public void OnPressTutorialButton()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void OnPressBossfightButton()
    {
        SceneManager.LoadScene("The Cave");
    }

    public void OnPressCustomFight()
    {

        if (_customFight != null)
        {
            _customFight.SetActive(true);
        }

        if (_mainMenu != null)
            _mainMenu.SetActive(false);
    }

    public void OnPressMainMenu()
    {
        if(_mainMenu != null)
            _mainMenu.SetActive(true);

        if (_customFight != null)
            _customFight.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Update()
    {
        //Debug.Log(CustomGame.rangedBoss);
    }
}
