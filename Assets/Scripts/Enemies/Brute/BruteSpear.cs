using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BruteSpear : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _indicator;
    private float _currentScale = 1f;
    private float _maxScale = 5f;
    private float _growSpeed = 7.5f;
    private Vector3 _startScale;


    [SerializeField]
    private GameObject _spears;
    private float _spearSpeed = 30f;
    [SerializeField]
    private float _spearDamage = 10f;
    GameMode.attackType _attackType = GameMode.attackType.melee;

    BoxCollider _collider;

    bool _hasHit = false;

    void Start()
    {
        if( _indicator != null )
        {
            _startScale = _indicator.transform.localScale;
        }

        _collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentScale < _maxScale && _indicator != null)
        {
            _currentScale += Time.deltaTime * _growSpeed;
            _indicator.transform.localScale = new Vector3(_startScale.x * _currentScale, _startScale.y, _startScale.z * _currentScale);

        }
        else if(_spears != null && _indicator != null)
        {
            if (_spears.transform.position.y <= _indicator.transform.position.y)
            {
                _spears.transform.Translate(0, Time.deltaTime * _spearSpeed, 0);
                if (_collider != null)
                {
                    _collider.center += new Vector3(0, Time.deltaTime * _spearSpeed, 0);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    const string FRIENDLY_TAG = "Friendly";
    const string ENEMY_TAG = "Enemy";
    private void OnTriggerEnter(Collider other)
    {
        if (_hasHit) return;
        //make sure we only hit friendly or enemies
        if (other.tag != FRIENDLY_TAG && other.tag != ENEMY_TAG)
            return;

        //only hit the opposing team
        if (other.tag == tag)
            return;


        Health otherHealth = other.GetComponent<Health>();
        if (otherHealth == null)
            otherHealth = other.GetComponentInChildren<Health>();

        if (otherHealth != null)
        {
            otherHealth.Damage(_spearDamage, _attackType);
            _hasHit = true;
        }
    }
}
