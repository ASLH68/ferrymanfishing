using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArduinoManager : MonoBehaviour
{
    public static ArduinoManager Instance;

    private void Awake()
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

    [SerializeField] private ArduinoThread _thread;
    [SerializeField] private PhysicalConnection _translator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public PhysicalConnection Translator
    {
        get
        {
            return _translator;
        }
    }

    public ArduinoThread Thread
    {
        get
        {
            return _thread;
        }
    }

    public void SendData(string data)
    {
        Thread.EnqueueData(data);
    }
}
