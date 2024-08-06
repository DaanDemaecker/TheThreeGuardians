using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BossHealthBars : MonoBehaviour
{
    [SerializeField] private Image _rangedHealthBar = null;

    [SerializeField] private Image _magicHealthBar = null;

    [SerializeField] private Image _meleeHealthBar = null;

    [SerializeField] private GameObject _managers = null;

    ArcherManager _archerManager = null;
    BruteManager _bruteManager = null;
    MageManager _mageManager = null;

    public void Start()
    {
        if(_managers != null)
        {
            _archerManager = _managers.GetComponent<ArcherManager>();
            _bruteManager = _managers.GetComponent<BruteManager>();
            _mageManager = _managers.GetComponent<MageManager>();
        }
    }

    public void Update()
    {
        SetPercentages();
    }

    void SetPercentages()
    {
        if(_rangedHealthBar != null && _archerManager != null)
        {
            float scale = 0;
            if (CustomGame.rangedBoss)
                scale = _archerManager.GetBossHealthPercentage();
            _rangedHealthBar.transform.localScale = new Vector3(scale , 1.0f, 1.0f);
        }

        if(_magicHealthBar != null && _mageManager != null)
        {
            float scale = 0;
            if(CustomGame.magicBoss)
                scale = _mageManager.GetHealthPercentage();
            _magicHealthBar.transform.localScale = new Vector3(scale , 1.0f, 1.0f);
        }

        if (_meleeHealthBar != null && _bruteManager != null)
        {
            float scale = 0;
            if (CustomGame.meleeBoss)
                scale = _bruteManager.GetHealthPercentage();
            _meleeHealthBar.transform.localScale = new Vector3(scale, 1.0f, 1.0f);
        }
    }
}
