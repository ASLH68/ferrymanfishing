using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class BlinkLightArduino : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _circle;

    private void Update()
    {
        if (Arduino.Instance.GetSerialPort().IsOpen)
        {
            try
            {
                BlinkLight(Arduino.Instance.GetSerialPort().ReadByte());
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }

    private void BlinkLight(int value)
    {
        if (value == 1)
        {
            _circle.color = Color.red;
        }
        else if (value == 0)
        {
            _circle.color = Color.white;
        }
    }
}
