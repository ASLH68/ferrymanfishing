using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDataTranslator : MonoBehaviour
{
    [SerializeField] private bool _debugDataMessages = false;
    [SerializeField] private Text text;
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

    [SerializeField] private float delayTime = 0.1f;

    private IEnumerator SendServo_Test()
    {
        TransmitCustomData("c");
        yield return new WaitForSeconds(delayTime);
        TransmitCustomData("d");
        yield return new WaitForSeconds(delayTime);
        TransmitCustomData("c");
    }

    public void DoServoTest()
    {
        StartCoroutine(SendServo_Test());
    }

    public ReceiveData GetReceivedData()
    {
        string data = ArduinoManager.Instance.Thread.ReceivedData;
        if (data == null) return ReceiveData.NoButtonsArePressed;
        if (_debugDataMessages)
        {
            Debug.Log("Receiving: " + data);
            if (data.Contains("g"))
            {
                text.text = "Receiving: Encoder";
            }
            else if (data.Contains("e") || data.Contains("f"))
            {
                text.text = "Receiving: Button";
            }
        }
        
        ArduinoManager.Instance.Thread.ReceivedData = null;
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
