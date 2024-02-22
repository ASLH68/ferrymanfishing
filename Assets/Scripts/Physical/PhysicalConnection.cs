using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalConnection : MonoBehaviour
{
    const char HapticOn = 'a';
    const char HapticOff = 'b';
    const char ButtonHigh = 'c';
    const char ButtonLow = 'd';
    const char EncoderIncreased = 'e';
    const char Servo0 = 'f';
    const char Servo1 = 'g';
    const char Servo2 = 'h';
    const char Servo3 = 'i';
    const char Servo4 = 'j';
    const char Servo5 = 'k';
    const char Servo6 = 'l';
    const char Servo7 = 'm';
    const char Servo8 = 'n';
    const char Servo9 = 'o';
    private static char[] ServoArray = {Servo0, Servo1, Servo2, Servo3, Servo4, Servo5, Servo6, Servo7, Servo8, Servo9};

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
    /// Sets the servo to one of 10 predetermined degree values.
    /// The values start at 0 degrees and go up to 50 degrees.
    /// To use this, pass in a 0-9 value. Any other values will be clamped.
    /// </summary>
    /// <param name="value"></param>
    public void SetServo(int value)
    {
        value = Math.Clamp(value, 0, 9);
        TransmitData(ServoArray[value].ToString());
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