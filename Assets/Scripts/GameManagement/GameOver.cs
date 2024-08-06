using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject _player = null;

    [SerializeField]
    private float _killBorder = -20;

    private int _currentScene = 0;

    private void Awake()
    {
        _currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if (_player == null)
        {
            TriggerGameOver();
        }
        else if (_player.transform.position.y < _killBorder)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        //include the namespace UnityEnginge.SceneManagement
        SceneManager.LoadScene(_currentScene);

    }
}
