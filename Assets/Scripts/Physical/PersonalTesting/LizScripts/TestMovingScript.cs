using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovingScript : MonoBehaviour
{
    [SerializeField] private TestDataTranslator _data;
    [Space]
    [SerializeField] private float _moveSpeed;
    private bool _onGreen, _onRed;

    private void Update()
    {
        if (_data.GetReceivedData() == TestDataTranslator.ReceiveData.LeftButtonIsPressed)
        {
            MoveCube(-1);
        }
        else if (_data.GetReceivedData() == TestDataTranslator.ReceiveData.RightButtonIsPressed)
        {
            MoveCube(1);
        }

        TransmitData();
    }

    private void TransmitData()
    {
        if (_onGreen)
        {
            _data.TransmitDataToArduino(TestDataTranslator.TransmittableData.PlayerIsOnGreen);
        }
        else if(_onRed)
        {
            _data.TransmitDataToArduino(TestDataTranslator.TransmittableData.PlayerIsOnRed);
        }
        else
        {
            _data.TransmitDataToArduino(TestDataTranslator.TransmittableData.PlayerIsOnNothing);
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
