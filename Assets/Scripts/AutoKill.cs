using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKill : MonoBehaviour
{
    [SerializeField]
    private float _killTIme = 5;
    void Start()
    {
        Invoke(KILL_METHODNAME, _killTIme);
    }

    protected const string KILL_METHODNAME = "Kill";

    void Kill()
    {
        Destroy(gameObject);
    }
}
