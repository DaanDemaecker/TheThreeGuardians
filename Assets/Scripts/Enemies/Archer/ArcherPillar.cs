using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherPillar : MonoBehaviour
{
    [SerializeField]
    private GameObject _smallArcherTemplate = null;

    [SerializeField]
    private GameObject _bossTemplate = null;

    [SerializeField]
    private GameObject _spawnPoint = null;

    private Archer _currentArcher = null;

    bool _hasBoss = false;
    public bool hasBoss
    {
        get { return _hasBoss; }
    }

    bool _bossDied = false;
    public bool bossDied
    {
        get { return _bossDied; }
    }

    private void Update()
    {
        if(_hasBoss && _currentArcher == null)
        {
            _hasBoss = false;
            _bossDied = true;
        }
    }

    public bool IsFree()
    {
        //check if the pillar has an archer on it
        if (_currentArcher == null)
            return true;
        else
            return false;
    }

    public float GetBossHealthPercentage()
    {
        if(_currentArcher == null)return 0;
        else return _currentArcher.GetHealthPercentage();
    }

    public void SpawnBoss()
    {
        //spawn the boss on this pillar
        if (_bossTemplate == null)
            return;

        _currentArcher = Instantiate(_bossTemplate, _spawnPoint.transform).GetComponent<Archer>();
        _hasBoss = true;
    }

    public Archer GetBoss()
    {
        //return the boss so he can be moved to another pillar and spawn a small archer in it's place
        if(_currentArcher != null)
        {
            if(_currentArcher.isBoss)
            {
                Archer archer = _currentArcher;
                _hasBoss = false;
                SpawnSmall();
                return archer;
            }
        }
        return null;
    }

    public void PlaceBoss(Archer boss)
    {
        //place the boss on this pillar
        _currentArcher = boss;

        if (_currentArcher == null)
        {
            return;
        }

        _currentArcher.Teleport(_spawnPoint.transform.position);
        _hasBoss = true;
    }

    public void SpawnSmall()
    {
        //spawn a small archer on this pillar
        _currentArcher = Instantiate(_smallArcherTemplate, _spawnPoint.transform).GetComponent<Archer>();
    }


}
