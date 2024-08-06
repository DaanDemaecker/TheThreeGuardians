using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : BasicWeapon
{
    [SerializeField] private GameObject _bulletTemplate = null;
    [SerializeField] private Transform _fireSocket = null;
    [SerializeField] private AudioSource _bowReleaseFX = null;

    const string _down = "MousePressed";

    const string _up = "MouseReleased";

    const string _idle = "Idle";

    const string _draw = "Draw";

    const string _drawn = "Drawn";

    private Arrow _currentArrow = null;

    private const float _arrowSpeed = 50.0f;

    // Update is called once per frame
    void Update()
    {

        if(_currentArrow != null)
        {
            _currentArrow.SetTransform(_fireSocket);
        }
    }
    override public bool IsIdle()
    {
        if (_animator != null)
            return _animator.GetCurrentAnimatorStateInfo(0).IsName(_idle) && !_animator.IsInTransition(0);
        else return base.IsIdle();
    }

    public override void Activate()
    {
        if (!IsIdle()) return;

        if (_animator != null)
        {
            _animator.SetTrigger(_down);
        }

        _currentArrow = Instantiate(_bulletTemplate, _fireSocket).GetComponent<Arrow>();
        if (_currentArrow != null)
        {
            _currentArrow.SetRoot(_fireSocket);
            _currentArrow.tag = this.tag;
            _currentArrow.damage = _damage;
        }

    }
    public override void Release()
    {
        float animationPercentage = 1;
        if (_animator != null)
        {
            if(_animator.GetCurrentAnimatorStateInfo(0).IsName(_draw))
            {
                animationPercentage = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            _animator.SetTrigger(_up);
        }


        if(_currentArrow != null)
        {
            Vector3 direction = _fireSocket.transform.forward;
            float offsetAngle = -5.0f;
            direction = Quaternion.AngleAxis(offsetAngle, _fireSocket.transform.right) * direction;
            _currentArrow.Release(_arrowSpeed, animationPercentage, direction);
            _currentArrow = null;
        }
        if (_bowReleaseFX != null)
            _bowReleaseFX.Play();
    }

}
