using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    private GameObject _playerTarget = null;

    [SerializeField]
    GameObject _shoulder = null;

    [SerializeField]
    GameObject _hand = null;

    [SerializeField]
    GameObject _weaponTemplate = null;

    [SerializeField]
    int _damage = 20;

    BasicWeapon _weapon = null;

    [SerializeField]
    int _maxCooldown = 8;
    float _cooldown = 0.0f;

    [SerializeField]
    bool _isBoss = false;

    [SerializeField]
    float _minDistance = 15.0f;

    private Health _health = null;

    public float GetHealthPercentage()
    {
        if (_health == null)
            return 1.0f;
        else
            return _health.HealthPercentage;
    }

    public bool isBoss
    {
        get { return _isBoss; }
    }

    void Awake()
    {
        //setup weapon and put it in the hand slot
        if (_weaponTemplate != null && _hand != null)
        {
            var weaponObject = Instantiate(_weaponTemplate, _hand.transform, true);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localRotation = Quaternion.identity;
            _weapon = weaponObject.GetComponent<BasicWeapon>();
            _weapon.damage = _damage;
        }

        //get the player as target
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        if (player) SetTarget(player.gameObject);

        _health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerTarget != null)
        {
            //player's position
            Vector3 desiredLookatPoint = _playerTarget.transform.position;

            //save the y level of the player for later
            float playerHeight = desiredLookatPoint.y;
            
            //set the y of the desiredlookatpoint equal to the y from the archer so that he only rotates around 
            desiredLookatPoint.y = transform.position.y;
            transform.LookAt(desiredLookatPoint, Vector3.up);

            if (_shoulder != null)
            {
                float horizontalDistance = Mathf.Sqrt(Mathf.Pow(transform.position.x - _playerTarget.transform.position.x, 2)
                + Mathf.Pow(transform.position.z - _playerTarget.transform.position.z, 2));

                float factor = 10.0f;

                float offset = 1.0f;
                if(horizontalDistance >= _minDistance) offset += horizontalDistance/factor;
                else offset -= horizontalDistance/factor;

                desiredLookatPoint.y = playerHeight + offset;

                _shoulder.transform.LookAt(desiredLookatPoint);
            }
        }

        if (_weapon != null)
        {
            if (_weapon.IsIdle())
            {
                _weapon.Activate();
                _cooldown = Random.Range(0, _maxCooldown);
            }
            else if(_cooldown >=  0)
            {
                _cooldown -= Time.deltaTime;
                if(_cooldown <= 0 )
                {
                    _weapon.Release();
                }
            }
            
        }
    }

    public void SetTarget(GameObject player)
    {
        _playerTarget = player;
    }

    public void Teleport(Vector3 position)
    {
        transform.position = position;
    }
}
