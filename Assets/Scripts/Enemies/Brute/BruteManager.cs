using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _bossSpawn;

    [SerializeField]
    private GameObject _bossTemplate;

    private Brute _brute;
    bool _bossSpawned;
    bool _bossDied;

    private GameObject _playerTarget = null;

    [SerializeField]
    private GameObject _spearTemplate = null;
    private float _minCooldown = 1f;
    private float _maxCooldown = 3f;
    private float _cooldown = 0f;

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
        if(_bossTemplate != null && _bossSpawn != null)
        {
            _brute = Instantiate(_bossTemplate, _bossSpawn.transform).GetComponent<Brute>();
            if(_brute != null)
            {
                _bossSpawned = true;
                _brute.transform.parent = null;
            }
        }
    }

    public void Update()
    {
        if(_bossSpawned && !_bossDied)
        {
            if (_brute == null)
            {
                _bossDied = true;
                _cooldown = Random.Range(_minCooldown, _maxCooldown);
            }
        }
        
        if(_bossDied)
        {
            if(_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
                if(_cooldown <= 0)
                {
                    SpawnSpear();
                    _cooldown = Random.Range(_minCooldown, _maxCooldown);
                }
            }
        }
    }

    public void SpawnSpear()
    {
        if (_spearTemplate != null && _playerTarget != null)
        {
            Instantiate(_spearTemplate, GetGroundPos(_playerTarget.transform.position), Quaternion.identity);
        }
    }

    public float GetHealthPercentage()
    {
        if (_brute != null)
        {
            return _brute.GetHealthPercentage();
        }
        else
        {
            if (!_bossSpawned)
                return 1.0f;
            return 0.0f;
        }
    }

    static readonly string[] RAYCAST_MASK = { "Ground", "StaticLevel", "DynamicLevel" };
    Vector3 GetGroundPos(Vector3 startPos)
    {
        Ray downRay = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(startPos, Vector3.down, out hit, 50.0f, LayerMask.GetMask(RAYCAST_MASK)))
        {
            return hit.point;
        }

        return startPos;
    }
}
