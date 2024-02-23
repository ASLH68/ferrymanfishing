using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using System.IO;

public class ArduinoThread : MonoBehaviour
{
    [SerializeField] private bool _debugExceptions;
    [SerializeField] private bool _debugSendReceiveData;

    [Header("Arduino")]
    [SerializeField] private int _baudRate;
    [SerializeField] private int _readTimeout;
    [SerializeField] private string _portName;
    private Arduino _arduino;
    private Thread _thread;
    private Queue _rxQueue;
    private Queue _txQueue;
    private bool _threadActive = true;

    private string _receivedData;
    private string _lastTransmitData;

    public string ReceivedData
    {
        get
        {
            return _receivedData;
        }
    }

    private void Start()
    {
        StartThread();
    }

    private void Update()
    {
        UpdateReceivedData();
    }

    public void EnqueueData(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return;
        }

        if (data == _lastTransmitData)
        {
            return;
        }

        if (_debugSendReceiveData)
        {
            Debug.Log("Transmitting data: " + data);
        }

        _lastTransmitData = data;
        _txQueue.Enqueue(data);
    }

    private void UpdateReceivedData()
    {
        if (_rxQueue.Count == 0)
        {
            return;
        }

        string data = (string)_rxQueue.Dequeue();
        if (data == "")
        {
            return;
        }

        if (data == _receivedData)
        {
            return;
        }

        if (_debugSendReceiveData)
        {
            Debug.Log("Receiving data: " + data);
        }

        _receivedData = data;
    }

    private void StartThread()
    {
        _rxQueue = Queue.Synchronized(new Queue());
        _txQueue = Queue.Synchronized(new Queue());
        _thread = new Thread(ThreadLoop);
        _thread.Start();
    }

    private void ThreadLoop()
    {
        _arduino = new Arduino(_baudRate, _readTimeout, _portName);

        Debug.Log("Thread started.");

        while (ThreadActive())
        {
            if (_txQueue.Count != 0)
            {
                string message = (string)_txQueue.Dequeue();
                SendToSerialPort(message);
            }

            string result = ReadFromSerialPort();
            if (result != null)
            {
                _rxQueue.Enqueue(result);
            }
        }

        Debug.Log("Thread ended.");
    }

    private void SendToSerialPort(string message)
    {
        try
        {
            _arduino.DataStream.Write(message);
        }
        catch(Exception e)
        {
            if (_debugExceptions)
            {
                Debug.Log(e);
            }
        }
    }

    private string ReadFromSerialPort()
    {
        //Data recieved is like: "0,0" for left right not pressed, "1,0" for left pressed right released, and so forth.
        try
        {
            return _arduino.DataStream.ReadLine();
        }
        catch (TimeoutException e)
        {
            if (_debugExceptions)
            {
                Debug.Log(e);
            }

            return null;
        }
        catch (IOException e)
        {
            if (_debugExceptions)
            {
                Debug.Log(e);
            }

            return null;
        }
    }

    public bool ThreadActive()
    {
        lock (this)
        {
            return _threadActive;
        }
    }

    [ContextMenu("Stop Thread")]
    public void StopThread()
    {
        lock (this)
        {
            _threadActive = false;
        }
    }

    private void OnDisable()
    {
        _arduino.DataStream.Close();
        StopThread();
    }

    public void ClearReceivedData()
    {
        if (string.IsNullOrEmpty(_receivedData)) return;
        _receivedData = null;
    }
}
