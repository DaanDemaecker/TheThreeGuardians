using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : BasicWeapon
{
    [SerializeField] private GameObject _bulletTemplate = null;
    [SerializeField] private Transform _fireSocket = null;

    [SerializeField] private float _fireRate = 25.0f;
    private float _fireTimer = 0.0f;

    const string _down = "MousePressed";

    const string _up = "MouseReleased";

    const string _idle = "Idle";
    const string _attacking = "Attack";

    // Update is called once per frame
    void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_attacking) && !_animator.IsInTransition(0))
        {
            if (_fireTimer > 0.0f)
                _fireTimer -= Time.deltaTime;

            if (_fireTimer <= 0.0f)
            {
                _fireTimer = 1.0f / _fireRate;
                FireProjectile();
            }
        }
    }
    override public bool IsIdle()
    {
        if(_animator != null)
            return _animator.GetCurrentAnimatorStateInfo(0).IsName(_idle) && !_animator.IsInTransition(0);
        else return base.IsIdle();
    }

    public override void Activate()
    {
        if (_animator != null)
            _animator.SetTrigger(_down);
    }

    public override void Release()
    {
        if (_animator != null)
            _animator.SetTrigger(_up);
    }

    private void FireProjectile()
    {
        //no bullet to fire
        if (_bulletTemplate == null)
            return;

        //no firesocket
        if (_fireSocket == null)
            return;

        BasicProjectile projectile = Instantiate(_bulletTemplate, _fireSocket.position, _cameraDirection).GetComponent<BasicProjectile>();
        if(projectile != null)
        {
            projectile.tag = this.tag;
            projectile.damage = _damage;
        }
    }
}
