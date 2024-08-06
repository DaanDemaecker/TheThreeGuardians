using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float _startHealth = 10;

    private float _currentHealth = 0;

    [SerializeField]
    private Color _flickerColor = Color.white;
    [SerializeField]
    private float _flickerDuration = 0.1f;

    private Color _startColor;
    private Material _attachedMaterial;

    const string COLOR_PARAMETER = "_Color";

    [SerializeField]
    private AudioSource _hitfx;

    [SerializeField]
    private GameMode.attackType _type = GameMode.attackType.melee;
    public GameMode.attackType AttackType
    {
        get { return _type; }
        set { _type = value; }
    }

    [SerializeField]
    private bool _isPlayer = false;

    public float HealthPercentage
    {
        get
        {
            return _currentHealth / _startHealth;
        }
    }

    private void Awake()
    {
        _currentHealth = _startHealth;
        Renderer renderer = transform.GetComponentInChildren<Renderer>();
        if (renderer)
        {
            //This will actually behind the scenes create a new instance of a material, use renderer.sharedmaterial to
            //Get the actual material use (be careful as this will change it for all objects using it)
            if (_isPlayer)
                _attachedMaterial = renderer.sharedMaterial;
            else _attachedMaterial = renderer.material;

            if (_attachedMaterial)
                _startColor = _attachedMaterial.GetColor(COLOR_PARAMETER);
        }
    }

    public bool Damage(float amount, GameMode.attackType attackType)
    {
        if (_isPlayer)
        {
            if (attackType == _type)
                return false;
            else if(_hitfx != null)
                _hitfx.Play();
        }
        else if (!_isPlayer && attackType != _type)
            return false;

        _currentHealth -= amount;

        if (_currentHealth <= 0)
            Kill();

        if (_attachedMaterial)
        {
            _attachedMaterial.SetColor(COLOR_PARAMETER, _flickerColor);
            Invoke(RESET_COLOR_METHOD, _flickerDuration);
        }
        return true;
    }

    const string RESET_COLOR_METHOD = "ResetColor";

    private void ResetColor()
    {
        if (!_attachedMaterial) return;

        _attachedMaterial.SetColor(COLOR_PARAMETER, _startColor);
    }

    void Kill()
    {
        ResetColor();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_attachedMaterial != null) return;

        //since we created a new material in the start, we should clean it up
        _attachedMaterial.color = _startColor;
        if(!_isPlayer)
            Destroy(_attachedMaterial);
    }

    public void ChangeColor(Color color)
    {
        if (!_attachedMaterial)
        {
            return;
        }

        _startColor = color;    
        //_attachedMaterial.SetColor(COLOR_PARAMETER, _startColor);
        _attachedMaterial.color = _startColor;
    }
}
