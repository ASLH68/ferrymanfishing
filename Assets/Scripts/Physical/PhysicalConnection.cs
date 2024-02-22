using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalConnection : MonoBehaviour
{
    private const char HapticOn = 'a';
    private const char HapticOff = 'b';
    private const char ServoHigh = 'c';
    private const char ServoLow = 'd';
    private const char ButtonHigh = 'e';
    private const char ButtonLow = 'f';
    private const char EncoderIncreased = 'g';

    /// <summary>
    /// Invoked when the rotary encoder increases.
    /// Be sure to unsubscribe from the action when finished.
    /// </summary>
    public Action OnRotaryEncoderIncreased;
    [field: SerializeField] public bool ButtonPressed { get; private set; }

    private void Update()
    {
        //receive
        HandleReceiving();
    }

    private void HandleReceiving()
    {
        string data = ArduinoManager.Instance.Thread.ReceivedData;
        if (string.IsNullOrEmpty(data)) return;
        char first = data[0];
        switch (first)
        {
            case ButtonHigh:
                ButtonPressed = true;
                break;
            case ButtonLow:
                ButtonPressed = false;
                break;
            case EncoderIncreased:
                OnRotaryEncoderIncreased?.Invoke();
                break;
        }

        ArduinoManager.Instance.Thread.ClearReceivedData();
    }

    /// <summary>
    /// Turns the haptic motors on or off. Will need to be wrapped in a Co-routine for quick pulses.
    /// </summary>
    /// <param name="on">Whether the haptic will be on.</param>
    public void ToggleHapticMotor(bool on)
    {
        TransmitData(on ? HapticOn.ToString() : HapticOff.ToString());
    }

    /// <summary>
    /// Sets the servo to be to its high position (defined in arduino code).
    /// </summary>
    public void SetServoHigh()
    {
        TransmitData(ServoHigh.ToString());
    }

    /// <summary>
    /// Sets the servo to be in its low position (defined in arduino code).
    /// </summary>
    public void SetServoLow()
    {
        TransmitData(ServoLow.ToString());
    }

    /// <summary>
    /// Transmits a string to the arduino. Expected strings are a single char.
    /// </summary>
    /// <param name="msg">The string to send to the arduino.</param>
    private void TransmitData(string msg)
    {
        ArduinoManager.Instance.SendData(msg);
    }
}
