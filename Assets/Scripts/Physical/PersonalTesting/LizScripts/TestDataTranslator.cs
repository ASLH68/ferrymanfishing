using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataTranslator : MonoBehaviour
{
    [SerializeField] private bool _debugDataMessages = false;

    public enum ReceiveData
    { 
        LeftButtonIsPressed,
        RightButtonIsPressed,
        NoButtonsArePressed,
    }

    public enum TransmittableData
    {
        PlayerIsOnGreen,
        PlayerIsOnRed,
        PlayerIsOnNothing,
    }

    public void TransmitDataToArduino(TransmittableData data)
    {
        if (_debugDataMessages)
        {
            Debug.Log("Sending data: " + data);
        }

        switch (data)
        {
            case TransmittableData.PlayerIsOnGreen:
                ArduinoManager.Instance.Thread.DataToTransmit = "1";
                break;
            case TransmittableData.PlayerIsOnRed:
                ArduinoManager.Instance.Thread.DataToTransmit = "2";
                break;
            case TransmittableData.PlayerIsOnNothing:
                ArduinoManager.Instance.Thread.DataToTransmit = "0";
                break;
            default:
                ArduinoManager.Instance.Thread.DataToTransmit = "0";
                break;
        }
    }

    public void TransmitCustomData(string data)
    {
        if (_debugDataMessages && data != null)
        {
            Debug.Log("Sending data: " + data);
        }

        ArduinoManager.Instance.Thread.DataToTransmit = data;
    }

    public ReceiveData GetReceivedData()
    {
        string data = ArduinoManager.Instance.Thread.ReceivedData;
        if (_debugDataMessages && data != null)
        {
            Debug.Log("Receiving: " + data);
        }

        switch (ArduinoManager.Instance.Thread.ReceivedData)
        {
            case "1":
                return ReceiveData.LeftButtonIsPressed;
            case "2":
                return ReceiveData.RightButtonIsPressed;
            default:
                return ReceiveData.NoButtonsArePressed;
        }
    }
}
