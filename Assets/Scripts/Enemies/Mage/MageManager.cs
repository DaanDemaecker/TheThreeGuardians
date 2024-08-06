using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _bossSpawn;

    [SerializeField]
    private List<GameObject> _positions;

    [SerializeField]
    private GameObject _bossTemplate;

    [SerializeField]
    private GameObject _magicBarierTemplate = null;

    [SerializeField]
    private GameObject _magicMeteorTemplate = null;

    private float _attackCooldown = 5.5f;
    private float _attackMinCooldown = 3.5f;
    private float _attackTimer = 0.0f;

    private Mage _mage;
    bool _bossSpawned;
    bool _bossDied;

    private float _moveCooldown = 7.5f;
    private float _moveTimer = 0.0f;

    private GameObject _playerTarget = null;

    public bool bossDied
    {
        get { return _bossDied; }
    }

    public void Awake()
    {
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();

        if (player) _playerTarget = player.gameObject;
    }

    public void Begin()
    {
        if (_bossTemplate != null && _bossSpawn != null)
        {
            _mage = Instantiate(_bossTemplate, _bossSpawn.transform).GetComponent<Mage>();
            if (_mage != null)
            {
                _bossSpawned = true;
                _mage.transform.parent = null;
                if(_playerTarget != null)
                    _mage.SetTarget(_playerTarget);
            }

            _moveTimer = _moveCooldown;
        }
    }

    public float GetHealthPercentage()
    {
        if (_mage != null)
        {
            return _mage.GetHealthPercentage();
        }
        else
        {
            if (!_bossSpawned)
                return 1.0f;
            return 0.0f;
        }
    }

    public void Update()
    {
        if(!_bossDied && _bossSpawned)
        {
            if (_mage == null)
            {
                _bossDied = true;
                _attackTimer = Random.Range(_attackMinCooldown, _attackCooldown);
            }

            if(_moveTimer >= 0)
            {
                _moveTimer -= Time.deltaTime;
                if(_moveTimer <= 0)
                {
                    NewPos();
                    _moveTimer = _moveCooldown;
                }
            }
        }
        else
        {
            if (_attackTimer > 0.0f)
            {
                _attackTimer -= Time.deltaTime;
                if (_attackTimer <= 0.0f)
                {
                    Attack();
                    _attackTimer = Random.Range(_attackMinCooldown, _attackCooldown);
                }
            }
        }
    }

    public void NewPos()
    {
        if (_playerTarget != null)
        {
            float maxDistance = 0;
            int maxIndex = 0;
            for (int i = 0; i < _positions.Count; i++)
            {
                float distance = Vector3.Distance(_positions[i].transform.position, _playerTarget.transform.position);
                if(distance > maxDistance)
                {
                    maxDistance = distance;
                    maxIndex = i;
                }
            }
            _mage.transform.position = _positions[maxIndex].transform.position;
        }
    }

    private void Attack()
    {
        GameObject startPos = _positions[Random.Range(0, _positions.Count)];

        int attack = Random.Range(0, 2);
        switch (attack)
        {
            case 0:
                if (_playerTarget != null && _magicBarierTemplate != null)
                {
                    Instantiate(_magicBarierTemplate, startPos.transform.position, GetDirectionToPlayer(startPos.transform.position, _playerTarget.transform.position));
                }
                break;
            case 1:
                if (_playerTarget != null && _magicMeteorTemplate != null)
                {
                    MagicMeteor meteor = Instantiate(_magicMeteorTemplate, startPos.transform.position + Vector3.up * 4, Quaternion.identity).GetComponent<MagicMeteor>();
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
