using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using System.IO;

public class TestThreadingScript : MonoBehaviour
{
    [SerializeField] private bool _debugExceptions;

    [Header("Arduino")]
    [SerializeField] private int _baudRate;
    [SerializeField] private int _readTimeout;
    [SerializeField] private float _readWriteInterval;
    [SerializeField] private string _portName;
    private Arduino _arduino;
    private Thread _thread;
    private Queue _rxQueue;
    private Queue _txQueue;
    private string _receivedData;
    private string _dataToTransmit;
    private bool _threadActive = true;

    public string ReceivedData
    {
        get
        {
            return _receivedData;
        }
    }

    public string DataToTransmit
    {
        set
        {
            _dataToTransmit = value;
        }
    }

    private void Start()
    {
        StartThread();

        InvokeRepeating(nameof(TransmitData), 0f, _readWriteInterval);
    }

    private void Update()
    {
        ReceiveRxQueue();
    }

    private void TransmitData()
    {
        SendToTXQueue(_dataToTransmit);
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

    public void SendToTXQueue(string msg)
    {
        if (msg == null)
        {
            return;
        }

        _txQueue.Enqueue(msg);
    }

    private string ReadRXQueue()
    {
        if (_rxQueue.Count == 0)
        {
            return null;
        }

        return (string)_rxQueue.Dequeue();
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

    private void ReceiveRxQueue()
    {
        string data = ReadRXQueue();
        if (data == null)
        {
            return;
        }

        _receivedData = data;
    }

    private void OnDisable()
    {
        _arduino.DataStream.Close();
        StopThread();
    }
}
