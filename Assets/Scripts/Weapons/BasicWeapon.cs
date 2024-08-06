using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour
{
    [SerializeField]
    protected GameMode.attackType _attackType;

    [SerializeField]
    protected int _damage = 10;

    public int damage
    {
        get { return _damage; } 
        set { _damage = value; }
    }

    protected Animator _animator = null;

    protected Quaternion _cameraDirection = Quaternion.identity;
    public Quaternion CameraDirection
    {
        get { return _cameraDirection; }
        set { _cameraDirection = value; }
    }

    public GameMode.attackType AttackType
    {
        get { return _attackType; }
        set { _attackType = value; }
    }

    public virtual void Activate()
    {

    }

    public virtual void Release()
    {

    }

    void Start()
    {
        _animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual public bool IsIdle()
    {
        return true;
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

        if (otherHealth != null)
        {
            otherHealth.Damage(_damage, _attackType);
        }
    }
}
