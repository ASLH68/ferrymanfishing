using System;
using System.Collections;
using System.IO;
using System.Threading;
using UnityEngine;

public class OLD_ArduinoThread
{
    private Thread _thread;
    private Queue _rxQueue;
    private Queue _txQueue;
    private Arduino _arduino;

    private int _baudRate;
    private int _readTimeout;
    private string _portName;
    private bool _threadActive;

    private bool _debugExceptions = false;

    public OLD_ArduinoThread(int baudRate, int readTimeout, string portName)
    {
        _baudRate = baudRate;
        _readTimeout = readTimeout;
        _portName = portName;

        _rxQueue = Queue.Synchronized(new Queue());
        _txQueue = Queue.Synchronized(new Queue());
        _thread = new Thread(ThreadLoop);
    }

    #region Accessors

    public void StartThread()
    {
        if (_threadActive)
        {
            Debug.LogWarning("Thread is already running!");
            return;
        }

        SetThreadActive(true);
        _thread.Start();
    }

    public void TransmitMessage(string messageToTransmit)
    {
        if (messageToTransmit == null)
        {
            return;
        }

        _txQueue.Enqueue(messageToTransmit);
    }

    public string ReadEnqueuedMessage()
    {
        if (_rxQueue.Count == 0)
        {
            return null;
        }


        return (string)_rxQueue.Dequeue();
    }

    public void StopThread()
    {
        SetThreadActive(false);
    }

    public Arduino GetArduino()
    {
        return _arduino;
    }

    #endregion

    #region Threading Functionality

    private void ThreadLoop()
    {
        _arduino = new Arduino(_baudRate, _readTimeout, _portName);

        Debug.Log($"Thread started.");

        while (GetThreadActive())
        {
            if (_txQueue.Count != 0)
            {
                string messageInQueue = (string)_txQueue.Dequeue();
                TransmitToArduino(messageInQueue);
            }

            string receivedString = ReceiveFromArduino();
            if (receivedString != null)
            {
                _rxQueue.Enqueue(receivedString);
            }
        }

        Debug.Log($"Thread stopped.");
    }

    private bool GetThreadActive()
    {
        lock (this)
        {
            return _threadActive;
        }
    }

    private void SetThreadActive(bool active)
    {
        lock (this)
        {
            _threadActive = active;
        }
    }

    private void TransmitToArduino(string stringToTransmit)
    {
        try
        {
            _arduino.DataStream.Write(stringToTransmit);
        }
        catch (Exception e)
        {
            if (_debugExceptions)
            {
                Debug.Log(e);
            }
        }
    }

    private string ReceiveFromArduino()
    {
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

    #endregion
}