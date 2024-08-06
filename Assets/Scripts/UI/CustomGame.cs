using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CustomGame : MonoBehaviour
{
    [SerializeField]
    Toggle _rangedToggle = null;
    [SerializeField]
    Toggle _magicToggle = null;
    [SerializeField]
    Toggle _meleeToggle = null;
    private void Start()
    {
        if (_rangedToggle != null)
        {
            _rangedToggle.isOn = _rangedBoss;
        }
        if (_magicToggle != null)
        {
            _magicToggle.isOn = _magicBoss;
        }
        if (_meleeToggle != null)
        {
            _meleeToggle.isOn = _meleeBoss;
        }
    }

    public void Update()
    {
        if(_rangedToggle != null)
        {
            _rangedBoss = _rangedToggle.isOn;
        }
        if(_meleeToggle != null)
        {
            _meleeBoss = _meleeToggle.isOn;
        }
        if(_magicToggle != null)
        {
            _magicBoss = _magicToggle.isOn;
        }
    }

    static bool _meleeBoss = true;
    public static bool meleeBoss
    {
        get { return _meleeBoss; }
        set { _meleeBoss = value; }
    }

    static bool _magicBoss = true;
    public static bool magicBoss
    {
        set { _magicBoss = value; }
        get { return _magicBoss; }
    }

    static bool _rangedBoss = true;
    public static bool rangedBoss
    {
        set { _rangedBoss = value; }
        get { return _rangedBoss; }
    }
}
