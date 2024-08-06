using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _meleeWeaponTemplate = null;
    [SerializeField]
    private GameObject _meleeSocket = null;

    [SerializeField]
    private GameObject _rangedWeaponTemplate = null;
    [SerializeField]
    private GameObject _rangedSocket = null;

    [SerializeField]
    private GameObject _magicWeaponTemplate = null;
    [SerializeField]
    private GameObject _magicSocket = null;

    [SerializeField]
    private GameObject _orbSocket = null;
    [SerializeField]
    private GameObject _orbTemplate = null;

    [SerializeField]
    private GameObject _handSocket = null;

    [SerializeField]
    private int _meleeDamage = 25;
    [SerializeField]
    private int _magicDamage = 5;
    [SerializeField]
    private int _rangedDamage = 50;

    private BasicWeapon _meleeWeapon = null;
    private BasicWeapon _rangedWeapon = null;
    private BasicWeapon _magicWeapon = null;

    private BasicWeapon _currentWeapon = null;
    private GameObject _currentWeaponSocket = null;

    GameMode.attackType _currentAttackType = GameMode.attackType.melee;
    public GameMode.attackType AttackType
    {
        get { return _currentAttackType; }
    }

    const string SCROLLWHEEL = "WeaponSelection";
    const string PRIMARYATTACK = "PrimaryAttack";

    private bool _mouseDown = false;

    void Awake()
    {
        const string friendlyTag = "Friendly";
        
        if (_meleeWeaponTemplate != null && _handSocket != null)
        {
            var weaponObject = Instantiate(_meleeWeaponTemplate, _handSocket.transform, true);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localRotation = Quaternion.identity;
            _meleeWeapon = weaponObject.GetComponent<BasicWeapon>();
            _meleeWeapon.tag = friendlyTag;
            _meleeWeapon.damage = _meleeDamage;
            _currentWeapon = _meleeWeapon;
            _currentWeaponSocket = _meleeSocket;
        }

        if (_rangedWeaponTemplate != null && _rangedSocket != null)
        {
            var weaponObject = Instantiate(_rangedWeaponTemplate, _rangedSocket.transform, true);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localRotation = Quaternion.identity;
            _rangedWeapon = weaponObject.GetComponent<BasicWeapon>();
            _rangedWeapon.tag = friendlyTag;
            _rangedWeapon.damage = _rangedDamage;
        }

        if (_magicWeaponTemplate != null && _magicSocket != null)
        {
            var weaponObject = Instantiate(_magicWeaponTemplate, _magicSocket.transform, true);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localRotation = Quaternion.identity;
            _magicWeapon = weaponObject.GetComponent<BasicWeapon>();
            _magicWeapon.tag = friendlyTag;
            _magicWeapon.damage = _magicDamage;
        }
    }

    private void Update()
    {
        if (Time.timeScale <= 0)
            return;
        float scrollMovement = Input.GetAxis(SCROLLWHEEL);
        if (scrollMovement != 0 && _currentWeapon.IsIdle())
        {
            SetWeapon((int)(scrollMovement * 10));
        }

        bool mouseDown = Input.GetAxis(PRIMARYATTACK) > 0.0f;

        if(_currentWeapon != null)
        {
            if(mouseDown && !_mouseDown)
            {
                _currentWeapon.Activate();
            }
            
            if(!mouseDown && _mouseDown)
            {
                _currentWeapon.Release();
            }
        }
        _mouseDown = mouseDown;
        
    }


    private void SetWeapon(int value)
    {

        if (_handSocket == null || _currentWeapon == null ||
            _meleeWeapon == null || _meleeSocket == null ||
            _rangedWeapon == null || _rangedSocket == null ||
            _magicWeapon == null || _magicSocket == null)
        {
            return;
        }

        if(_currentWeapon.GetComponentInChildren<TrailRenderer>() != null)
        _currentWeapon.GetComponentInChildren<TrailRenderer>().enabled = false;

        _currentWeapon.transform.parent = _currentWeaponSocket.transform;
        _currentWeapon.transform.position = _currentWeaponSocket.transform.position;
        _currentWeapon.transform.rotation = _currentWeaponSocket.transform.rotation;

        _mouseDown = false;

         int newType = (int) _currentAttackType + value;

        if (newType < 0) newType = 2;
        else if (newType > 2) newType = 0;

        _currentAttackType = (GameMode.attackType) newType;

        switch(_currentAttackType)
        {
            case GameMode.attackType.melee:
                _currentWeapon = _meleeWeapon;
                _currentWeaponSocket = _meleeSocket;
                break;
            case GameMode.attackType.ranged:
                _currentWeapon = _rangedWeapon;
                _currentWeaponSocket = _rangedSocket;
                break;
            case GameMode.attackType.magic:
                _currentWeapon = _magicWeapon;
                _currentWeaponSocket = _magicSocket;
                break;
            default:
                break;
        }

        _currentWeapon.transform.parent = _handSocket.transform;
        _currentWeapon.transform.position = _handSocket.transform.position;
        _currentWeapon.transform.rotation = _handSocket.transform.rotation;

        if (_currentWeapon.GetComponentInChildren<TrailRenderer>() != null)
            _currentWeapon.GetComponentInChildren<TrailRenderer>().enabled = true;
    }

    public void SetCameraDirection(Quaternion direction)
    {
        if(_currentWeapon != null)
            _currentWeapon.CameraDirection = direction;
    }

    public void ThrowOrb(Quaternion direction, MovementBehaviour behaviour)
    {
        if (_orbTemplate != null && _orbSocket != null)
        {
            GameObject orb = Instantiate(_orbTemplate, _orbSocket.transform.position, direction);
            orb.GetComponent<TeleportOrb>().playerMovement = behaviour;
        }
    }


}
