using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovingScript : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private void Update()
    {
        if (ArduinoManager.Instance.Translator.GetReceivedData() == TestDataTranslator.ReceiveData.LeftButtonIsPressed)
        {
            MoveCube(-1);
        }
        else if (ArduinoManager.Instance.Translator.GetReceivedData() == TestDataTranslator.ReceiveData.RightButtonIsPressed)
        {
            MoveCube(1);
        }
    }

    private void MoveCube(int dir)
    {
        transform.position += Vector3.right * dir * _moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "GreenOn")
        {
            ArduinoManager.Instance.Translator.TransmitDataToArduino(TestDataTranslator.TransmittableData.PlayerIsOnGreen);
        }
        if (collision.gameObject.name == "RedOn")
        {
            ArduinoManager.Instance.Translator.TransmitDataToArduino(TestDataTranslator.TransmittableData.PlayerIsOnRed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "GreenOn" || collision.gameObject.name == "RedOn")
        {
            ArduinoManager.Instance.Translator.TransmitDataToArduino(TestDataTranslator.TransmittableData.PlayerIsOnNothing);
        }
    }
}
