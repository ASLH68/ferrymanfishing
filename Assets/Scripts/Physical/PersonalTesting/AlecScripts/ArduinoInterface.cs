using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.IO.Ports;
using System.Management;
using System.Threading;
using Microsoft.Win32;

public class ArduinoInterface : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private string _serialPortName = "COM5";
    [SerializeField] private int _baudRate = 9600;

    [Header("Servo Motor")]
    [SerializeField] private int _forwardRotationIncrement = 25;
    [SerializeField] private int _backwardRotationIncrement = 5;

    // Start is called before the first frame update
    private SerialPort mogPort;
    private int frameCount;
    private Thread thread;
    private Queue outputQueue;
    private Queue inputQueue;
    private string port;
    private bool looping = true;

    void Start()
    {
        
        print(port);
        // mogPort = new SerialPort(port, 9600);
        // mogPort.ReadTimeout = 50;
        // mogPort.WriteTimeout = 50;
        // mogPort.Open();
        //StartCoroutine(ReadFromArduinoAsync(null, null));
        StartThread();
    }

    private void StartThread()
    {
        outputQueue = Queue.Synchronized(new Queue());
        inputQueue = Queue.Synchronized(new Queue());
        thread = new Thread(ThreadLoop);
        thread.Start();
    }

    public void SendToArduinoThreaded(string command)
    {
        if (command == null) return;
        outputQueue.Enqueue(command);
    }

    public string ReadFromArduinoThreaded()
    {
        if (inputQueue.Count == 0)
        {
            return null;
        }

        return (string) inputQueue.Dequeue();
    }

    public void StopThread()
    {
        lock (this)
        {
            looping = false;
        }
    }

    public bool IsLooping()
    {
        lock (this)
        {
            return looping;
        }
    }

    void OnMessageArrived(string msg)
    {
        
    }
    
    private void ThreadLoop()
    {
        mogPort = new SerialPort(_serialPortName, _baudRate);
        mogPort.ReadTimeout = 50;
        mogPort.DtrEnable = true;
        mogPort.RtsEnable = true;
        mogPort.Open();

        while (IsLooping())
        {
            if (outputQueue.Count != 0)
            {
                string command = (string) outputQueue.Dequeue();
                WriteToArduino(command);
            }

            string result = ReadFromArduino(50);
            if (result != null)
            {
                inputQueue.Enqueue(result);
            }
        }

        mogPort.Close();
    }

    public static string AutodetectArduinoPort()
    {
        List<string> comports = new List<string>();
        RegistryKey rk1 = Registry.LocalMachine;
        RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
        string temp;
        foreach (string s3 in rk2.GetSubKeyNames())
        {
            RegistryKey rk3 = rk2.OpenSubKey(s3);
            foreach (string s in rk3.GetSubKeyNames())
            {
                if (s.Contains("VID") && s.Contains("PID"))
                {
                    RegistryKey rk4 = rk3.OpenSubKey(s);
                    foreach (string s2 in rk4.GetSubKeyNames())
                    {
                        RegistryKey rk5 = rk4.OpenSubKey(s2);
                        if ((temp = (string) rk5.GetValue("FriendlyName")) != null && temp.Contains("Arduino"))
                        {
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            if (rk6 != null && (temp = (string) rk6.GetValue("PortName")) != null)
                            {
                                comports.Add(temp);
                            }
                        }
                    }
                }
            }
        }

        if (comports.Count > 0)
        {
            foreach (string s in SerialPort.GetPortNames())
            {
                if (comports.Contains(s))
                    return s;
            }
        }

        return "COM6";
    }


    private void WriteToArduino(string msg)
    {
        try
        {
            mogPort.WriteLine(msg);
        }
        catch (Exception e)
        {
            print(e);
            throw;
        }
    }

    public string ReadFromArduino(int timeout = 0)
    {
        mogPort.ReadTimeout = timeout;
        try
        {
            return mogPort.ReadLine();
        }
        catch (TimeoutException e)
        {
            Debug.Log(e);
            return null;
        }
        catch (IOException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public IEnumerator ReadFromArduinoAsync(Action<string> callback, Action fail = null,
        float timeOut = float.PositiveInfinity)
    {
        DateTime init = DateTime.Now;
        TimeSpan diff;
        do
        {
            string dataIn = null;
            try
            {
                dataIn = mogPort.ReadLine();
                print(dataIn);
            }
            catch (TimeoutException e)
            {
                Debug.LogException(e);
                dataIn = null;
            }

            if (dataIn != null)
            {
                callback?.Invoke(dataIn);
                yield break;
            }
            else
            {
                yield return null;
            }

            var nowTime = DateTime.Now;
            diff = nowTime - init;
        } while (diff.Milliseconds < timeOut);

        fail?.Invoke();
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        // if (frameCount % 2 == 0)
        // {
        //     WriteToArduino("SET_SERVO_180");
        // }
        // else if (frameCount % 3 == 0)
        // {
        //     WriteToArduino("SET_SERVO_90");
        // }
        //WriteToArduino(" ");
        // ReadFromArduino();
        SendToArduinoThreaded($"{_forwardRotationIncrement}|{_backwardRotationIncrement}");
        print(ReadFromArduinoThreaded());
        frameCount++;
    }

    void OnDisable()
    {
        StopThread();
        mogPort.Close();
    }
}