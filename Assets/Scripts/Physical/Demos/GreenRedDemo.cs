using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenRedDemo : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private bool _onGreen;
    private bool _onRed;

    private void Update()
    {
        SendLightStatus();

        MoveCubeViaButton(ArduinoDataManager.Instance.ButtonStatus);
    }

    private void MoveCubeViaButton(string message)
    {
        if (message == "1")
        {
            MoveCube(-1);
        }
        else if (message == "2")
        {
            MoveCube(1);
        }
    }

    private void MoveCube(int dir)
    {
        transform.position += Vector3.right * dir * _moveSpeed * Time.deltaTime;
    }

    private void SendLightStatus()
    {
        if (_onGreen)
        {
            ArduinoDataManager.Instance.LightStatus = "1";
        }
        else
        {
            ArduinoDataManager.Instance.LightStatus = "2";
        }

        if (_onRed)
        {
            ArduinoDataManager.Instance.LightStatus = "3";
        }
        else
        {
            ArduinoDataManager.Instance.LightStatus = "4";
        }
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