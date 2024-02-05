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

        StartCoroutine(MotorSpinCoroutine());
    }

    private void Update()
    {

    }

    private IEnumerator MotorSpinCoroutine()
    {
        while (true)
        {
            SpinMotor();

            yield return new WaitForSeconds(1f);
        }
    }

    [ContextMenu("Invoke Spin")]
    private void SpinMotor()
    {
        _a.GetThread().WriteMessageThreaded($"{_forwardsRot}|{_backwardsRot}");

        string msg = _a.GetThread().ReadMessageThreaded();
        if (msg != null)
        {
            Debug.Log("Recieving: " + msg);
        }
    }
}
