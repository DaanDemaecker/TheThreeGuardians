using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMeteor : BasicProjectile
{
    private float _currentScale = 1;
    private float _maxScale = 5;
    private float _growFactor = 2;

    private GameObject _playerTarget = null;
    void Start()
    {
        
    }

    protected override void FixedUpdate()
    {
        if (_currentScale < _maxScale)
        {
            _currentScale += Time.deltaTime * _growFactor;
            transform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
            if(_currentScale >= _maxScale && _playerTarget != null)
            {
                transform.LookAt(_playerTarget.transform.position + Vector3.up * 1.5f);
            }
        }
        else
        {
            if (!WallDetection())
                transform.position += transform.forward * Time.deltaTime * _speed;
        }
    }

    public void SetTarget(GameObject player)
    {
        _playerTarget = player;
    }
}
