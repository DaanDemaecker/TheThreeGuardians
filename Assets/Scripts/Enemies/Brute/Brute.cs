using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Brute : MonoBehaviour
{
    enum State
    {
        Spinning,
        Idle
    }

    State _state = State.Idle;

    private GameObject _playerTarget = null;

    [SerializeField]
    private GameObject _hand = null;

    [SerializeField]
    private GameObject _weaponTemplate = null;

    [SerializeField]
    private GameObject _particles = null;

    [SerializeField]
    private float _minDistance = 2.5f;

    [SerializeField]
    private float _movementSpeed = 7.0f;

    [SerializeField]
    private float _angularSpeed = 900.0f;

    [SerializeField]
    private int _damage = 20;

    BasicWeapon _weapon = null;

    [SerializeField]
    private float _idleCooldown = 5.0f;
    [SerializeField]
    private float _maxSpinCooldown = 7.0f;
    private float _cooldown = 0.0f;

    private Health _health = null;

    [SerializeField]
    public AudioSource _attackSound = null;
    public float GetHealthPercentage()
    {
        if (_health == null)
            return 1.0f;
        else
            return _health.HealthPercentage;
    }

    void Start()
    {
        //spawn weapon and put it in the hand slot
        if (_weaponTemplate != null && _hand != null)
        {
            var weaponObject = Instantiate(_weaponTemplate, _hand.transform, true);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localRotation = Quaternion.identity;
            _weapon = weaponObject.GetComponent<BasicWeapon>();
            if (_weapon != null)
            {
                _weapon.damage = _damage;
                _weapon.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }

        //particles should be inactive at the start
        if(_particles != null)
            _particles.SetActive(false);

        //get player as target
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        if (player) _playerTarget = player.gameObject;

        _health = GetComponent<Health>();
        _cooldown = _idleCooldown;
    }

    void Update()
    {
        switch(_state)
        {
            case State.Idle:
                if(_cooldown >= 0.0f)
                {
                    _cooldown -= Time.deltaTime;
                    //particles should be turned off half a second before next attack
                    if (_cooldown < 0.5f && _particles != null)
                        _particles.SetActive(false);
                    if (_cooldown <= 0.0f)
                        SetState(State.Spinning);
                }
                break;
            case State.Spinning:
                if (_cooldown >= 0.0f)
                {
                    _cooldown -= Time.deltaTime;
                    if(_cooldown <= 0.0f)
                        SetState(State.Idle);
                }

                transform.Rotate(Vector3.up, _angularSpeed * Time.deltaTime);
                if (_playerTarget != null)
                {
                    //move towards the player
                    Vector3 playerPosition = _playerTarget.transform.position;
                    Vector3 direction = playerPosition - transform.position;
                    direction.y = 0;

                    //stop moving towards player when in a certain range
                    if (direction.magnitude >= _minDistance)
                    {
                        transform.position += direction.normalized * _movementSpeed * Time.deltaTime;
                    }
                }

                break;
            default:
                break;
        }
    }

    private void SetState(State state)
    {
        _state = state;

        switch(state)
        {
            case State.Idle:
                _cooldown = _idleCooldown;
                if(_weapon != null)
                    _weapon.transform.rotation = Quaternion.Euler(Vector3.zero);
                if (_particles != null)
                    _particles.SetActive(true);
                if (_attackSound != null)
                {
                    Debug.Log("stop play");
                    _attackSound.Stop();
                }
                break;
            case State.Spinning:
                _cooldown = _maxSpinCooldown;
                if (_weapon != null)
                {
                    Quaternion newAngle = transform.rotation;
                    newAngle *= Quaternion.AngleAxis(90, _weapon.transform.right);
                    _weapon.transform.rotation = newAngle;
                }
                if(_attackSound != null)
                {
                    Debug.Log("play");
                    _attackSound.Play();
                }
                else if(_attackSound == null)
                {
                    Debug.Log("no attacksound");
                }
                break;
            default:
                break;
        }
    }
}
