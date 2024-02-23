using Microsoft.Win32;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class Arduino
{
    private SerialPort _dataStream;

    public SerialPort DataStream
    {
        get
        {
            return _dataStream;
        }
    }

    public Arduino(int baudRate, int readTimeout, string portName)
    {
        string port = portName;
        if (portName == "")
        {
            port = GetConnectedSerialPort();
        }

        _dataStream = new SerialPort(port, baudRate);
        _dataStream.ReadTimeout = readTimeout;
        _dataStream.DtrEnable = true;
        _dataStream.RtsEnable = true;
        _dataStream.Open();
    }

    private string GetConnectedSerialPort()
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
                {
                    Debug.Log($"Arduino identified in port: {s}.");
                    return s;
                }
            }
        }

        Debug.LogWarning($"COM Port not detected! Is the Arduino plugged in? If yes, click for more solutions. \n" +
            $"1. Manually enter your COM port into the Arduino Thread field to bypass autodetection. \n" +
            $"2. If you do not have the Arduino IDE, download it for Windows here: https://www.arduino.cc/en/software. \n" +
            $"Integration for other operation systems is not implemented. \n");
        return null;
    }
}
