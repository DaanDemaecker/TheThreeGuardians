using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public enum attackType
    {
        melee,
        ranged,
        magic,
        none
    }

    [SerializeField]
    private float _gameStart = 3.0f;

    [SerializeField] private GameObject _managers = null;

    ArcherManager _archerManager = null;
    BruteManager _bruteManager = null;
    MageManager _mageManager = null;

    private void Awake()
    {
        Invoke(SPAWNBOSES_METHOD, _gameStart);

        if (_managers != null)
        {
            _archerManager = _managers.GetComponent<ArcherManager>();
            _mageManager = _managers.GetComponent<MageManager>();
            _bruteManager = _managers.GetComponent<BruteManager>();
        }
    }

    const string SPAWNBOSES_METHOD = "SpawnBosses";

    void SpawnBosses()
    {
        if (_archerManager != null && CustomGame.rangedBoss)
        {
            _archerManager.Begin();
        }

        if(_mageManager != null && CustomGame.magicBoss)
        {
            _mageManager.Begin();
        }

        if(_bruteManager != null && CustomGame.meleeBoss)
        {
            _bruteManager.Begin();
        }
    }

    private void OnDestroy()
    {
    }

    public void Update()
    {
        if (_archerManager != null && _bruteManager != null && _mageManager != null)
        {
            if((_archerManager.bossDied || !CustomGame.rangedBoss) &&
                (_mageManager.bossDied || !CustomGame.magicBoss) &&
                (_bruteManager.bossDied || !CustomGame.meleeBoss))
                SceneManager.LoadScene("WinScreen");
        }
    }


}
