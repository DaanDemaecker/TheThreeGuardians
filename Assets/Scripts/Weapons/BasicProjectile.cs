using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField]
    protected float _speed = 30.0f;
    [SerializeField]
    protected float _lifeTime = 10.0f;

    [SerializeField]
    protected float _damage = 5;

    [SerializeField]
    protected GameObject _hitFX;
    public float damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    [SerializeField]
    GameMode.attackType _attackType = GameMode.attackType.melee;

    public virtual void Awake()
    {
        Invoke(KILL_METHODNAME, _lifeTime);
    }

    protected virtual void FixedUpdate()
    {
        if (!WallDetection())
            transform.position += transform.forward * Time.deltaTime * _speed;
    }

    static readonly string[] RAYCAST_MASK = { "StaticLevel", "DynamicLevel", "Ground" };
    protected bool WallDetection()
    {
        Ray collisionRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(collisionRay,
            Time.deltaTime * _speed, LayerMask.GetMask(RAYCAST_MASK)))
        {
            Kill();
            return true;
        }

        return false;
    }

    protected const string KILL_METHODNAME = "Kill";

    void Kill()
    {
        Destroy(gameObject);
    }

    const string FRIENDLY_TAG = "Friendly";
    const string ENEMY_TAG = "Enemy";
    private void OnTriggerEnter(Collider other)
    {
        //make sure we only hit friendly or enemies
        if (other.tag != FRIENDLY_TAG && other.tag != ENEMY_TAG)
            return;

        //only hit the opposing team
        if (other.tag == tag)
            return;

        

        Health otherHealth = other.GetComponent<Health>();
        if(otherHealth == null)
            otherHealth = other.GetComponentInChildren<Health>();

        if (otherHealth != null)
        {
            if(otherHealth.Damage(_damage, _attackType))
            {
                if (_hitFX != null && tag == FRIENDLY_TAG)
                    Instantiate(_hitFX, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
