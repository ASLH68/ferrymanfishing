using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using System.IO;

public class RedGreenTXRX_OneByte : MonoBehaviour
{
    [SerializeField] private bool _debugExceptions;

    [Header("Arduino")]
    [SerializeField] private int _baudRate;
    [SerializeField] private int _readTimeout;
    [SerializeField] private float _readWriteInterval;
    [SerializeField] private string _portName;
    private SerialPort _dataStream;
    private Thread _thread;
    private Queue _rxQueue;
    private Queue _txQueue;
    private string _recievedData;
    private bool _threadActive = true;

    [Header("Cube")]
    [SerializeField] private float _moveSpeed;
    private bool _onGreen;
    private bool _onRed;

    private void Start()
    {
        StartThread();

        InvokeRepeating("SendLightStatus", 0f, _readWriteInterval);
        InvokeRepeating("RecieveButtonStatus", 0f, _readWriteInterval);
        //InvokeRepeating("RecieveButtonStatus", 0f, _readWriteInterval);
    }

    #region Threading

    private void StartThread()
    {
        _rxQueue = Queue.Synchronized(new Queue());
        _txQueue = Queue.Synchronized(new Queue());
        _thread = new Thread(ThreadLoop);
        _thread.Start();
    }

    private void ThreadLoop()
    {
        _dataStream = new SerialPort(_portName, _baudRate);
        _dataStream.ReadTimeout = _readTimeout;
        _dataStream.DtrEnable = true;
        _dataStream.RtsEnable = true;
        _dataStream.Open();

        Debug.Log("Thread started.");

        while (ThreadActive())
        {
            if (_txQueue.Count != 0)
            {
                string command = (string)_txQueue.Dequeue();
                SendToSerialPort(command);
            }

            string result = ReadFromSerialPort();
            if (result != null)
            {
                _rxQueue.Enqueue(result);
            }
        }

        Debug.Log("Thread ended.");
    }

    private void SendToSerialPort(string msg)
    {
        try
        {
            _dataStream.WriteLine(msg);
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
            return _dataStream.ReadLine();
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

    private void SendToTXQueue(string msg)
    {
        if (msg == null)
        {
            return;
        }

        Debug.Log("Sending " + msg);

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

    private void OnDisable()
    {
        _dataStream.Close();
        StopThread();
    }

    #endregion

    private void Update()
    {
        RecieveButtonStatus();

        if (_recievedData == "1")
        {
            MoveCube(-1);
        }
        else if (_recievedData == "2")
        {
            MoveCube(1);
        }
    }

    private void RecieveButtonStatus()
    {
        string data = ReadRXQueue();
        if (data == null)
        {
            return;
        }

        _recievedData = data;
        Debug.Log(_recievedData);
    }

    private void SendLightStatus()
    {
        if (_onGreen)
        {
            SendToTXQueue("0");
        }
        if (_onRed)
        {
            SendToTXQueue("1");
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
