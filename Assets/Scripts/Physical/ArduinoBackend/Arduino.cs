using Microsoft.Win32;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class Arduino : MonoBehaviour
{
    public static Arduino Instance;

    [Header("USB Port")]
    [SerializeField] private string _manualPortName = "";

    [Header("Read/Write Speeds")]
    [SerializeField] private int _readTimeout = 5000;
    [SerializeField] private int _baudRate = 9600;

    private ArduinoThread _arduinoThread;

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

        string port = _manualPortName;
        if (port == "")
        {
            port = GetArduinoPortWindows();
        }

        _arduinoThread = new ArduinoThread(port, _baudRate, _readTimeout);
    }

    private void OnDisable()
    {
        _arduinoThread.Stop();
    }

    private string GetArduinoPortWindows()
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
                        if ((temp = (string)rk5.GetValue("FriendlyName")) != null && temp.Contains("Arduino"))
                        {
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            if (rk6 != null && (temp = (string)rk6.GetValue("PortName")) != null)
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

    public ArduinoThread GetThread()
    {
        return _arduinoThread;
    }
}
