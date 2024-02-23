using UnityEngine;

public class OLD_ArduinoDataManager : MonoBehaviour
{
    public static OLD_ArduinoDataManager Instance;

    [SerializeField] private float _dataTransmitInterval;

    [Header("Arduino Data")]
    [SerializeField] private int _baudRate;
    [SerializeField] private int _readTimeout;
    [SerializeField] private string _portName;

    private OLD_ArduinoThread _arduinoThread;
    private string _buttonStatus;
    private string _lightStatus;

    public OLD_ArduinoThread ArduinoThread
    {
        get
        {
            return _arduinoThread;
        }
    }
    public string ButtonStatus
    {
        get
        {
            return _buttonStatus;
        }
    }
    public string LightStatus
    {
        set
        {
            _lightStatus = value;
        }
        get
        {
            return _lightStatus;
        }
    }

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
//Instance = this;

        _arduinoThread = new OLD_ArduinoThread(_baudRate, _readTimeout, _portName);
        _arduinoThread.StartThread();
    }

    private void Start()
    {
        InvokeRepeating("SendLightStatus", 0f, _dataTransmitInterval);
        InvokeRepeating(nameof(ReceiveButtonStatus), 0f, _dataTransmitInterval);
    }

    private void SendLightStatus()
    {
        _arduinoThread.TransmitMessage(_lightStatus);
    }

    private void ReceiveButtonStatus()
    {
        string data = _arduinoThread.ReadEnqueuedMessage();
        if (data == null)
        {
            return;
        }

        _buttonStatus = data;
    }

    private void OnDisable()
    {
        _arduinoThread.StopThread();
        _arduinoThread.GetArduino().DataStream.Close();
    }
}
