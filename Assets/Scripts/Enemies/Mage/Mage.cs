using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    [SerializeField]
    private GameObject _magicBarierTemplate = null;

    [SerializeField]
    private GameObject _magicMeteorTemplate = null;

    [SerializeField]
    private GameObject _meteorSpawn = null;

    private float _attackCooldown = 5.5f;
    private float _attackMinCooldown = 3.5f;
    private float _attackTimer = 0.0f;

    private Health _health = null;

    private GameObject _playerTarget = null;

    public float GetHealthPercentage()
    {
        if (_health == null)
            return 1.0f;
        else
            return _health.HealthPercentage;
    }

    void Start()
    {
        _health = GetComponent<Health>();

        _attackTimer = Random.Range(_attackMinCooldown, _attackCooldown);
    }
    public void SetTarget(GameObject player)
    {
        _playerTarget = player;
    }

    private void Update()
    {
        if(_playerTarget != null)
        {
            Vector3 desiredLookatPoint = _playerTarget.transform.position;

            float playerHeight = desiredLookatPoint.y;

            desiredLookatPoint.y = transform.position.y;
            transform.LookAt(desiredLookatPoint, Vector3.up);

            if(_attackTimer > 0.0f)
            {
                _attackTimer -= Time.deltaTime;
                if(_attackTimer <= 0.0f)
                {
                    Attack();
                    _attackTimer = Random.Range(_attackMinCooldown, _attackCooldown);
                }
            }
        }
    }

    private void Attack()
    {
        int attack = Random.Range(0, 2);
        switch (attack)
        {
            case 0:
                if (_playerTarget != null && _magicBarierTemplate != null)
                {
                    Instantiate(_magicBarierTemplate, transform.position, GetDirectionToPlayer(transform.position, _playerTarget.transform.position));
                }
                break;
            case 1:
                if (_playerTarget != null && _magicMeteorTemplate != null && _meteorSpawn != null)
                {
                    MagicMeteor meteor = Instantiate(_magicMeteorTemplate, _meteorSpawn.transform.position, Quaternion.identity).GetComponent<MagicMeteor>();
                    if (meteor != null)
                        meteor.SetTarget(_playerTarget);
                }
                break;
        }
    }

    Quaternion GetDirectionToPlayer(Vector3 startPos, Vector3 playerPos)
    {
        Vector3 direction = playerPos - startPos;
        direction.y = 0;
        return Quaternion.LookRotation(direction);
    }
}
