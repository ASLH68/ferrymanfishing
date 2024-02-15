using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private TestThreadingScript _thread;
    [SerializeField] private TestDataTranslator _translator;

    public TestDataTranslator Translator
    {
        get
        {
            return _translator;
        }
    }

    public TestThreadingScript Thread
    {
        get
        {
            return _thread;
        }
    }
}
