using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class RedGreenTXRX : MonoBehaviour
{
    [Header("Arduino")]
    [SerializeField] private int _baudRate;
    [SerializeField] private string _portName;
    private SerialPort _dataStream;
    private string _recievedString;


    [Header("Visuals")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Color _greenLightColor, _redLightColor;

    [Header("References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _dataStream = new SerialPort(_portName, _baudRate);
        _dataStream.Open();
    }

    private void Update()
    {
        RecieveData();
    }

    private void RecieveData()
    {
        //Readline reads console output as a string
        _recievedString = _dataStream.ReadLine();

        //Data recieved is like: "0,0" for left right not pressed, "1,0" for left pressed right released, and so forth.
        int[] data = StringArrayToIntArray(_recievedString.Split(','));
        
        //If the left button is pressed
        if (data[0] == 1)
        {
            MoveCube(-1);
        }

        //If the right button is pressed
        if (data[1] == 1)
        {
            MoveCube(1);
        }
    }

    private int[] StringArrayToIntArray(string[] data)
    {
        int[] numbers = new int[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            numbers[i] = int.Parse(data[i]);
        }

        return numbers;
    }

    private void MoveCube(int dir)
    {
        transform.position += Vector3.right * dir * Time.deltaTime;
    }

    private void ChangeColor(Color c)
    {
        _spriteRenderer.color = c;
    }
}
