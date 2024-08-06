using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image _swordIcon = null;
    [SerializeField] private Image _staffIcon = null;
    [SerializeField] private Image _bowIcon = null;
    [SerializeField] private Image _dashCooldown = null;
    [SerializeField] private Image _teleportCooldown = null;

    [SerializeField] private Image _meleeArmor = null;
    [SerializeField] private Image _magicArmor = null;
    [SerializeField] private Image _rangedArmor = null;
    [SerializeField] private Image _meleeArmorCooldown = null;
    [SerializeField] private Image _magicArmorCooldown = null;
    [SerializeField] private Image _rangedArmorCooldown = null;

    [SerializeField] private Image _healthBar = null;

    PlayerAttackBehaviour _attackBehaviour = null;
    PlayerCharacter _character = null;
    void Start()
    {
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();

        if (player != null)
        {
            _attackBehaviour = player.GetComponent<PlayerAttackBehaviour>();
            if(_attackBehaviour != null)
            {
                SetWeaponOutline(_attackBehaviour.AttackType);
            }

            _character = player.GetComponent<PlayerCharacter>();
            if(_character != null)
            {
                SetPercentages();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackBehaviour != null)
        {
            SetWeaponOutline(_attackBehaviour.AttackType);
        }

        if(_character != null)
        {
            SetArmorOutline(_character.GetArmorType());
            SetPercentages();
        }
    }

    private void SetWeaponOutline(GameMode.attackType type)
    {
        if (_swordIcon == null || _staffIcon == null
            || _bowIcon == null) return;

        _swordIcon.enabled = type == GameMode.attackType.melee;
        _staffIcon.enabled = type == GameMode.attackType.magic;
        _bowIcon.enabled = type == GameMode.attackType.ranged;
    }

    private void SetArmorOutline(GameMode.attackType type)
    {
        if (_meleeArmor == null || _magicArmor == null
            || _rangedArmor == null) return;

        _meleeArmor.enabled = type == GameMode.attackType.melee;
        _magicArmor.enabled = type == GameMode.attackType.magic;
        _rangedArmor.enabled = type == GameMode.attackType.ranged;
    }


    private void SetPercentages()
    {
        if(_dashCooldown != null)
        {
            _dashCooldown.transform.localScale =
                new Vector3(1.0f, _character.GetDashCooldownPercentage(), 1.0f);
        }

        if(_teleportCooldown != null)
        {
            _teleportCooldown.transform.localScale =
                new Vector3(1.0f, _character.GetTeleportCooldownPercentage(), 1.0f);
        }

        float armorTimer = _character.GetArmorDuration();
        float armorCooldown = _character.GetArmorCooldownPercentage();
        float armorPercentage = 0;

        if(armorTimer > 0)
        {
            armorPercentage = armorTimer;
        }
        else if(armorCooldown != 0)
        {
            armorPercentage = 1.0f - armorCooldown;
        }


        if(_meleeArmorCooldown != null)
        {
            _meleeArmorCooldown.transform.localScale =
                new Vector3(1.0f, armorPercentage, 1.0f);
        }

        if(_magicArmorCooldown != null)
        {
            _magicArmorCooldown.transform.localScale =
                new Vector3(1.0f, armorPercentage, 1.0f);
        }

        if(_rangedArmorCooldown != null)
        {
            _rangedArmorCooldown.transform.localScale=
                new Vector3(1.0f, armorPercentage, 1.0f);
        }

        if(_healthBar != null)
        {
            _healthBar.transform.localScale = new Vector3(_character.GetHealthPercentage(), 1.0f, 1.0f);
        }
    }
}
