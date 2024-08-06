using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOrb : MonoBehaviour
{
    [SerializeField]
    float _speed = 20.0f;
    [SerializeField]
    private float _lifeTime = 5.0f;

    private Vector3 _velocity;

    MovementBehaviour _playerMovement;
    public MovementBehaviour playerMovement
    {
        get { return _playerMovement; }
        set { _playerMovement = value; }
    }

    const string KILL_METHODNAME = "Kill";
    private void Awake()
    {
        Invoke(KILL_METHODNAME, _lifeTime);

        _velocity = transform.forward * _speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!WallDetection())
        {
            transform.position += _velocity * Time.deltaTime;
        }
    }

    static readonly string[] RAYCAST_MASK = { "Ground", "StaticLevel", "DynamicLevel" };
    bool WallDetection()
    {
        Ray collisionRay = new Ray(transform.position, _velocity);
        Ray downRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(collisionRay,
            Time.deltaTime * _velocity.magnitude*3, LayerMask.GetMask(RAYCAST_MASK)) ||
            Physics.Raycast(downRay, 0.2f, LayerMask.GetMask(RAYCAST_MASK)))
        {
            _playerMovement.Teleport(transform.position);
            Kill();
            return true;
        }

        return false;
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
