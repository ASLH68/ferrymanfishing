using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Management;
using System.Threading;
using Microsoft.Win32;

public class ArduinoInterface : MonoBehaviour
{
    // Start is called before the first frame update
    private SerialPort mogPort;
    private int frameCount;

    void Start()
    {
        string port = AutodetectArduinoPort();
        mogPort = new SerialPort(port, 9600);
        mogPort.ReadTimeout = 50;
        mogPort.WriteTimeout = 50;
        mogPort.Open();
        //mogPort = new(, 9600);
    }

    void OnDisable()
    {
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
            mogPort.BaseStream.Flush();
        }
        catch (Exception e)
        {
            print(e);
            throw;
        }
    }

    [ContextMenu("READ")]
    private void ReadFromArduino()
    {
        string dataIN = mogPort.ReadLine();
        print(dataIN);
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
        WriteToArduino("GET_BUTTON_STATE");
        ReadFromArduino();
        frameCount++;
    }
}