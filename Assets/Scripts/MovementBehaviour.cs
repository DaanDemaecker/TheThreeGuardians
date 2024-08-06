using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 10.0f;

    [SerializeField]
    private float _jumpForce = 500.0f;

    private float _rayCastLength = 0.0f;

    private bool _canDoubleJump = true;

    [SerializeField]
    private float _dashMultiplier = 50.0f;

    [SerializeField]
    private float _dashTime = 0.3f;
    private float _dashTimer = 0.0f;

    private Vector3 _dashDirection = Vector3.zero;

    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;

    private Vector3 _desiredMovementDirection = Vector3.zero;

    private Vector3 _jumpVector = Vector3.zero;

    public Vector3 DesiredMovementDirection
    {
        get { return _desiredMovementDirection; }
        set { _desiredMovementDirection = value; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<CapsuleCollider>();
        _jumpVector = Vector3.up * _jumpForce;
        _rayCastLength = _collider.bounds.size.y/1.5f;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 movement = _desiredMovementDirection.normalized;
        movement *= _movementSpeed;
        movement.y = _rigidbody.velocity.y;

        if(_dashTimer > 0.0f)
        {
            _dashTimer -= Time.deltaTime;
            movement.y = 0.0f;
            movement += _dashDirection * _dashMultiplier;
        }

        _rigidbody.velocity = movement;
    }

    static readonly string[] RAYCAST_MASK = { "Ground", "StaticLevel", "DynamicLevel" };
    public void Jump(GameMode.attackType attackType)
    {
        if (_rigidbody != null)
        {
            Debug.DrawRay(_collider.transform.position, Vector3.up * -1 * _rayCastLength, Color.green, 2);

            Ray collisionRay = new Ray(_collider.transform.position, Vector3.up*-1);
            if (Physics.Raycast(collisionRay, _rayCastLength , LayerMask.GetMask(RAYCAST_MASK)))
            {
                Vector3 movement = _rigidbody.velocity;
                movement.y = _jumpForce;

                _rigidbody.velocity = movement;
                _canDoubleJump = true;
            }
            else if(attackType == GameMode.attackType.ranged && _canDoubleJump)
            {
                Vector3 movement = _rigidbody.velocity;
                movement.y = _jumpForce;

                _rigidbody.velocity = movement;
                _canDoubleJump = false;
            }
        }
    }

    public void Dash(Vector3 direction)
    {
        _dashDirection = direction;
        _dashTimer = _dashTime;
    }

    public void Teleport(Vector3 position)
    {
        transform.position = position;
    }
}
