using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovingScript : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private string _customSendCharacter = "c";
    private bool _onGreen, _onRed;

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

        TransmitData();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ArduinoManager.Instance.Translator.TransmitCustomData(_customSendCharacter);
        }
    }

    private void TransmitData()
    {
        if (_onGreen)
        {
            ArduinoManager.Instance.Translator.TransmitDataToArduino(TestDataTranslator.TransmittableData.PlayerIsOnGreen);
        }
        else if(_onRed)
        {
            ArduinoManager.Instance.Translator.TransmitDataToArduino(TestDataTranslator.TransmittableData.PlayerIsOnRed);
        }
        else
        {
            ArduinoManager.Instance.Translator.TransmitDataToArduino(TestDataTranslator.TransmittableData.PlayerIsOnNothing);
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
            _onGreen = true;
        }
        if (collision.gameObject.name == "RedOn")
        {
            _onRed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "GreenOn")
        {
            _onGreen = false;
        }
        if (collision.gameObject.name == "RedOn")
        {
            _onRed = false;
        }
    }
}
