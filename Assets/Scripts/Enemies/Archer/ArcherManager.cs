using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManager : MonoBehaviour
{
    [SerializeField]
    private List<ArcherPillar> _pillars = new List<ArcherPillar>();

    bool _started = false;


    [SerializeField]
    float _minTimer = 5.0f;
    [SerializeField]
    float _maxTimer = 10.0f;
    float _timer = -1.0f;

    int _bossIndex = 0;

    public bool _bossDied = false;
    public bool bossDied
    {
        get { return _bossDied; }
        set { _bossDied = value; }
    }

    public float GetBossHealthPercentage()
    {
        if (!_started)
        {
            return 1.0f;
        }
        else if (_bossDied)
        {
            return 0.0f;
        }
        else if (_pillars[_bossIndex].hasBoss)
        {
            return _pillars[_bossIndex].GetBossHealthPercentage();
        }

        return 0.0f;
    }

    public void Begin()
    {
        //set the game in action
        _started = true;
        SpawnBoss();
        _timer = Random.Range(_minTimer, _maxTimer);
    }

    private void Update()
    {
        _pillars.RemoveAll(s => s == null);

        if (!_bossDied)
        {
            //check all pillars if the boss died
            for (int i = 0; i < _pillars.Count; i++)
            {
               _bossDied = _bossDied || _pillars[i].bossDied;
            }
        }

        if(_started)
        {
            if (_timer >= 0.0f)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                _timer = Random.Range(_minTimer, _maxTimer);

                NewWave();
            }
        }
    }

    const string NEWWAVE_METHOD = "NewWave";

    private void SpawnBoss()
    {
        //spawn the boss on a random pillar
        int index = Random.Range(0, _pillars.Count);

        if(_pillars[index] != null && _pillars[index].IsFree())
        {
            _pillars[index].SpawnBoss();
            _bossIndex = index;
        }
    }

    private void NewWave()
     {
        List<ArcherPillar> freePillars =  new List<ArcherPillar>();

        for(int i = 0; i < _pillars.Count; i++)
        {
            if (_pillars[i].IsFree()) freePillars.Add(_pillars[i]);
        }

        if(freePillars.Count > 0)
        {
            //if the boss is dead, spawn an archer on a random pillar
            if(_bossDied)
            {
                freePillars[Random.Range(0, freePillars.Count - 1)].SpawnSmall();
            }
            //if the boss is alive, move the boss to a random pillar and leave behind a new archer
            else
            {
                int newIndex;
                do
                {
                    newIndex = Random.Range(0, _pillars.Count);
                }
                while(!freePillars.Contains(_pillars[newIndex]));

                _pillars[newIndex].PlaceBoss(_pillars[_bossIndex].GetBoss());
                _bossIndex = newIndex;
            }
        }
    }
}
