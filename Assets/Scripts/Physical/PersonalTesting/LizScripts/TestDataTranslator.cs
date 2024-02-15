using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataTranslator : MonoBehaviour
{
    [SerializeField] private TestThreadingScript _arduinoData;

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
        switch (data)
        {
            case TransmittableData.PlayerIsOnGreen:
                _arduinoData.DataToTransmit = "1";
                break;
            case TransmittableData.PlayerIsOnRed:
                _arduinoData.DataToTransmit = "2";
                break;
            case TransmittableData.PlayerIsOnNothing:
                _arduinoData.DataToTransmit = "0";
                break;
            default:
                _arduinoData.DataToTransmit = "0";
                break;
        }
    }

    public ReceiveData GetReceivedData()
    {
        switch (_arduinoData.ReceivedData)
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
