using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataTranslator : MonoBehaviour
{
    public enum ReceiveData
    { 
        LeftButtonIsPressed,
        RightButtonIsPressed,
        NoButtonsArePressed,
        NoDataIsReceived,
    }

    public enum TransmittableData
    {
        PlayerIsOnGreen,
        PlayerIsOnRed,
        PlayerIsOnNothing,
    }

    public void TransmitDataToArduino(TransmittableData data)
    {
        switch (data)
        {
            case TransmittableData.PlayerIsOnGreen:
                ArduinoManager.Instance.Thread.EnqueueData("1");
                break;
            case TransmittableData.PlayerIsOnRed:
                ArduinoManager.Instance.Thread.EnqueueData("2");
                break;
            case TransmittableData.PlayerIsOnNothing:
                ArduinoManager.Instance.Thread.EnqueueData("0");
                break;
            default:
                ArduinoManager.Instance.Thread.EnqueueData("");
                break;
        }
    }

    public void TransmitCustomData(string data)
    {
        ArduinoManager.Instance.Thread.EnqueueData(data);
    }

    public ReceiveData GetReceivedData()
    {
        string data = ArduinoManager.Instance.Thread.ReceivedData;

        switch (data)
        {
            case "1":
                return ReceiveData.LeftButtonIsPressed;
            case "2":
                return ReceiveData.RightButtonIsPressed;
            case "0":
                return ReceiveData.NoButtonsArePressed;
            default:
                return ReceiveData.NoDataIsReceived;
        }
    }
}
