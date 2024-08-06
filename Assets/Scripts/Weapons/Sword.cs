using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : BasicWeapon
{
    const string _swing = "Swing";

    const string _idle = "Idle";
    const string _toIdle = "ToIdle";

    [SerializeField]
    private GameObject _hitFX = null;
    

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate()
    {
        if (_animator != null)
            _animator.SetTrigger(_swing);
    }

    override public bool IsIdle()
    {
        if (_animator != null)
            return _animator.GetCurrentAnimatorStateInfo(0).IsName(_idle) && !_animator.IsInTransition(0);
        else return base.IsIdle();
    }


    const string FRIENDLY_TAG = "Friendly";
    const string ENEMY_TAG = "Enemy";
    private void OnTriggerEnter(Collider other)
    {
        //make sure the sword is swinging
        if(IsIdle())
        {
            return;
        }

        //make sure we only hit friendly or enemies
        if (other.tag != FRIENDLY_TAG && other.tag != ENEMY_TAG)
            return;

        //only hit the opposing team
        if (other.tag == tag)
            return;

        Health otherHealth = other.GetComponent<Health>();

        if(_hitFX != null)
        {
            Instantiate(_hitFX, transform.position, transform.rotation);
        }

        if (otherHealth != null)
        {
            otherHealth.Damage(_damage, _attackType);
        }
    }
}

