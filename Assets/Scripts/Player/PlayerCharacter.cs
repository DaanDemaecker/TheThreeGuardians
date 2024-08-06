using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    const string MOVEMENT_HORIZONTAL = "MovementHorizontal";
    const string MOVEMENT_VERTICAL = "MovementVertical";
    const string MOVEMENT_JUMP = "Jump";
    const string MouseX = "MouseX";
    const string MouseY = "MouseY";
    const string Dash = "Dash";
    const string Teleport = "Teleport";
    const string Armor1 = "Armor1";
    const string Armor2 = "Armor2";
    const string Armor3 = "Armor3";

    [SerializeField]
    private Color _baseColor = Color.red;
    [SerializeField]
    private Color _meleeColor = Color.white;
    [SerializeField]
    private Color _rangedColor = Color.white;
    [SerializeField]
    private Color _magicColor = Color.white;

    [SerializeField]
    private float _sensitivity = 500.0f;

    [SerializeField]
    private float _dashCooldown = 5.0f;
    private float _dashTimer = 0.0f;

    [SerializeField]
    private float _maxArmorCooldown = 5.0f;
    private float _armorCooldown = 0.0f;
    [SerializeField]
    private float _armorDuration = 5.0f;
    private float _armorTimer = 0.0f;

    protected MovementBehaviour _movementBehaviour;
    public float GetDashCooldownPercentage()
    {
        return _dashTimer / _dashCooldown;
    }

    public float GetTeleportCooldownPercentage()
    {
        return _teleportTimer/ _teleportCooldown;
    }

    public float GetArmorCooldownPercentage()
    {
        return _armorCooldown / _maxArmorCooldown;
    }

    public float GetArmorDuration()
    {
        return _armorTimer / _armorDuration;
    }

    [SerializeField]
    private float _teleportCooldown = 5.0f;
    private float _teleportTimer = 0.0f;

    private Camera _camera;
    [SerializeField]
    private GameObject _cameraTarget;
    [SerializeField]
    private GameObject _shoulder;

    private PlayerAttackBehaviour _attackBehaviour;
    private Health _health;

    public float GetHealthPercentage()
    {
        if (_health == null)
            return 1.0f;
        else
            return _health.HealthPercentage;
    }

    // Start is called before the first frame update
    void Start()
    {
        _movementBehaviour = GetComponent<MovementBehaviour>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _camera = GetComponentInChildren<Camera>();
        _attackBehaviour = GetComponent<PlayerAttackBehaviour>();
        _health = GetComponent<Health>();

        if (_health != null)
            _health.ChangeColor(_baseColor);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleMouseInput();

        _attackBehaviour.SetCameraDirection(_camera.transform.rotation);

        if(_dashTimer > 0.0f)
        {
            _dashTimer -= Time.deltaTime;
            if(_dashTimer < 0.0f)
                _dashTimer = 0.0f;
        }

        if(_teleportTimer > 0.0f)
        {
            _teleportTimer -= Time.deltaTime;
            if(_teleportTimer < 0.0f)
                _teleportTimer = 0.0f;
        }

        if(_armorCooldown > 0.0f)
        {
            _armorCooldown -= Time.deltaTime; ;
            if(_armorCooldown < 0)
            {
                _armorCooldown = 0;
            }
        }

        if(_armorTimer > 0.0f)
        {
            _armorTimer -= Time.deltaTime; ;
            if (_armorTimer <= 0)
            {
                _armorTimer = 0;
                if(_health != null)
                {
                    _health.AttackType = GameMode.attackType.none;
                    _health.ChangeColor(_baseColor);
                    _armorCooldown = _maxArmorCooldown;
                }
            }
        }
    }

    void HandleMovementInput()
    {
        if (Time.timeScale <= 0)
            return;

        if (_movementBehaviour == null) return;

        //movement
        float horizontalMovement = Input.GetAxis(MOVEMENT_HORIZONTAL);
        float verticalMovement = Input.GetAxis(MOVEMENT_VERTICAL);

        Vector3 movement = horizontalMovement * transform.right + verticalMovement * transform.forward;

        if (_attackBehaviour.AttackType == GameMode.attackType.melee && _dashTimer <= 0.0f
           && Input.GetButtonDown(Dash))
        {
            _dashTimer = _dashCooldown;
            Vector3 direction = movement;
            direction.y = 0;
            if(direction.x < float.Epsilon && direction.z < float.Epsilon)
            {
                direction = transform.forward;
            }
            _movementBehaviour.Dash(direction);
        }

        if(_attackBehaviour.AttackType == GameMode.attackType.magic && _teleportTimer <= 0.0f
            && Input.GetButtonDown(Teleport))
        {
            float angle = 30;
            Quaternion direction = _camera.transform.rotation;
            direction *= Quaternion.Euler(0, 0, angle);
            _attackBehaviour.ThrowOrb(direction, _movementBehaviour);
            _teleportTimer = _teleportCooldown;
        }

        if(_armorCooldown <= 0.0f && _armorTimer <= 0.0f)
        {
            if (Input.GetButtonDown(Armor1))
            {
                SwitchArmor(GameMode.attackType.melee);
            }
            if (Input.GetButtonDown(Armor2))
            {
                SwitchArmor(GameMode.attackType.magic);
            }
            if (Input.GetButtonDown(Armor3))
            {
                SwitchArmor(GameMode.attackType.ranged);
            }
        }

        _movementBehaviour.DesiredMovementDirection = movement;

        if (Input.GetButtonDown(MOVEMENT_JUMP)) _movementBehaviour.Jump(_attackBehaviour.AttackType);
     }

    void HandleMouseInput()
    {
        float horizontalMovement = Input.GetAxis(MouseX);
        float verticalMovement = Input.GetAxis(MouseY);

        if (horizontalMovement < float.MinValue) horizontalMovement = 0;
        if (verticalMovement < float.MinValue) verticalMovement = 0;

        transform.Rotate(new Vector3(0, horizontalMovement * _sensitivity * Time.deltaTime, 0));

        float angle = -verticalMovement * _sensitivity * Time.deltaTime;
        float angleMin = -50;
        float angleMax = 85;
        float angleBetween = Vector3.SignedAngle(_camera.transform.forward, _cameraTarget.transform.forward, _cameraTarget.transform.right * -1);
        float newAngle = Mathf.Clamp(angleBetween + angle, angleMin, angleMax);
        angle = newAngle - angleBetween;

        _camera.transform.LookAt(_cameraTarget.transform);
        _camera.transform.RotateAround(_cameraTarget.transform.position, _cameraTarget.transform.right, angle);

        _shoulder.transform.rotation = _camera.transform.rotation;
    }

    void SwitchArmor(GameMode.attackType type)
    {
        if (_health == null) return;
        if(_health.AttackType == type) return;

        _health.AttackType = type;
        _armorTimer = _armorDuration;

        switch(type)
        {
            case GameMode.attackType.melee:
                _health.ChangeColor(_meleeColor);
                break;
            case GameMode.attackType.magic:
                _health.ChangeColor(_magicColor);
                break;
            case GameMode.attackType.ranged:
                _health.ChangeColor(_rangedColor);
                break;
        }

    }

    public GameMode.attackType GetArmorType()
    {
        return _health.AttackType;
    }
}
