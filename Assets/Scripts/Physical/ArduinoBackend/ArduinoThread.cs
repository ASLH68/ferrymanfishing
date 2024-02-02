using System;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class ArduinoThread
{
    //Set true to output port exceptions
    private bool OUTPUT_EXCEPTIONS = false;

    private SerialPort _sp;
    private Thread _arduinoThread;
    private Queue _outputQueue, _inputQueue;
    private bool _looping = true;

    private string _portName;
    private int _baudRate;
    private int _readTimeout;
    private int _writeTimeout;

    public ArduinoThread(string portName, int baudRate, int readTimeout, int writeTimeout)
    {
        _portName = portName;
        _baudRate = baudRate;
        _readTimeout = readTimeout;
        _writeTimeout = writeTimeout;

        _outputQueue = Queue.Synchronized(new Queue());
        _inputQueue = Queue.Synchronized(new Queue());

        _arduinoThread = new Thread(ThreadLoop);
        _arduinoThread.Start();
    }

    private void ThreadLoop()
    {
        _sp = new SerialPort(_portName, _baudRate);
        _sp.ReadTimeout = _readTimeout;
        _sp.WriteTimeout = _writeTimeout;
        _sp.DtrEnable = true;
        _sp.RtsEnable = true;
        _sp.Open();

        while (IsLooping())
        {
            if (_outputQueue.Count != 0)
            {
                string writtenMessage = (string)_outputQueue.Dequeue();
                WriteMessage(writtenMessage);
            }

            string readMessage = ReadMessage(_readTimeout);
            if (readMessage != null)
            {
                _inputQueue.Enqueue(readMessage);
            }
        }

        Debug.Log("Thread ended.");
    }

    public void Stop()
    {
        lock (this)
        {
            _looping = false;
            _sp.Close();
        }
    }

    private void WriteMessage(string message)
    {
        try
        {
            _sp.WriteLine(message);
        }
        catch (Exception e)
        {
            if(OUTPUT_EXCEPTIONS) { Debug.Log(e); }
        }
    }

    public void WriteMessageThreaded(string message)
    {
        if (message == null)
        {
            return;
        }

        Debug.Log("Writing: " + message);

        _outputQueue.Enqueue(message);
    }

    private string ReadMessage(int timeout = 0)
    {
        _sp.ReadTimeout = timeout;
        try
        {
            string message = _sp.ReadLine();
            return message;
        }
        catch (TimeoutException e)
        {
            if (OUTPUT_EXCEPTIONS) { Debug.Log(e); }

            return null;
        }
        catch (IOException e)
        {
            if (OUTPUT_EXCEPTIONS) { Debug.Log(e); }

            return null;
        }
    }

    public string ReadMessageThreaded()
    {
        if (_inputQueue.Count == 0)
        {
            return null;
        }

        return (string)_inputQueue.Dequeue();
    }

    public void FlushSerialPort()
    {
        _sp.BaseStream.Flush();
    }

    private bool IsLooping()
    {
        lock (this)
        {
            return _looping;
        }
    }
}
