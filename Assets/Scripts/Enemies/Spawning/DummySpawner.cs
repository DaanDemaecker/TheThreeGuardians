using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject _dummyTemplate = null;

    GameObject _currentDummy = null;

    private float _cooldown = 5.0f;
    private float _timer = 0.0f;

    bool _spawned = false;

    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
                Spawn();
        }

        if(_currentDummy == null && _spawned)
        {
            _spawned = false;
            _timer = _cooldown;
        }
    }

    void Spawn()
    {
        if (_dummyTemplate != null)
        {
            _currentDummy = Instantiate(_dummyTemplate, transform);
            if(_currentDummy != null)_spawned = true;
        }
    }
}
