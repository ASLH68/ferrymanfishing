using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class MoveCubeWithArduino : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private void Update()
    {
        if (Arduino.Instance.GetBoard().IsOpen)
        {
            ReadFromArduino();
        }
        else
        {
            Debug.LogWarning("The SerialPort " + Arduino.Instance.GetBoard().PortName + " is not open.");
        }
    }

    private void ReadFromArduino()
    {
        try
        {
            //yellow button is pushed
            if (Arduino.Instance.GetBoard().ReadByte() == 1)
            {
                Debug.Log("Recieving " + Arduino.Instance.GetBoard().ReadByte() + " from Arduino.");
                transform.Translate(Vector3.left * Time.deltaTime * _moveSpeed);
            }

            //green button is pushed
            if (Arduino.Instance.GetBoard().ReadByte() == 2)
            {
                Debug.Log("Recieving " + Arduino.Instance.GetBoard().ReadByte() + " from Arduino.");
                transform.Translate(Vector3.right * Time.deltaTime * _moveSpeed);
            }
        }
        catch (System.Exception)
        {
            //Issue with serial port?
        }
    }
}