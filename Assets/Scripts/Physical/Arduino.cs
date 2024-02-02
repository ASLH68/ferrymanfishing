using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Arduino : MonoBehaviour
{
    public static Arduino Instance;

    private const string DEFAULT_PORTNAME = "COM5";
    private const int DEFAULT_READTIMEOUT = 5000;

    [SerializeField] private string _portName;
    [SerializeField] private int _readTimeout;
    private SerialPort _sp;

    private void PrepareSingleton()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        PrepareSingleton();

        if (_portName == "")
        {
            _portName = DEFAULT_PORTNAME;
        }
        _sp = new SerialPort(_portName, 9600);

        _sp.Open();

        if (_readTimeout == 0)
        {
            _readTimeout = DEFAULT_READTIMEOUT;
        }
        _sp.ReadTimeout = _readTimeout; //Smooth transition delay
    }

    private void OnDisable()
    {
        _sp.Close();
    }

    public SerialPort GetSerialPort()
    {
        return _sp;
    }
}
