using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieveTest1 : MonoBehaviour
{
    [Header("Servo Rotation")]
    [SerializeField] private int _forwardsRot;
    [SerializeField] private int _backwardsRot;

    private Arduino _a;

    private void Start()
    {
        _a = Arduino.Instance;
    }

    private void Update()
    {
        _a.GetThread().WriteMessageThreaded($"{_forwardsRot}|{_backwardsRot}");

        string msg = _a.GetThread().ReadMessageThreaded();
        if (msg != null)
        {
            Debug.Log(msg);
        }
    }
}
