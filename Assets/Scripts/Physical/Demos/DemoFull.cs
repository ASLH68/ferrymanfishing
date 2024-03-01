using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoFull : MonoBehaviour
{
    [SerializeField] private Text _debugText;
    [SerializeField] private string _textDefault = "Awaiting Data...";
    [SerializeField] private float _textHideTime = 1f;
    private float _curTextHideTime;
    private bool buttonWasPressed;
    private bool buttonPressed;
    private bool buttonState;
    private void Start()
    {
        ArduinoManager.Instance.Translator.OnRotaryEncoderIncreased += OnEncoderIncreased;

        _debugText.text = _textDefault;
    }

    private void Update()
    {
        DebugTextTimer();

        buttonPressed = ArduinoManager.Instance.Translator.ButtonPressed;
        if (buttonWasPressed && !buttonPressed)
            ArduinoManager.Instance.Translator.SetServo(0);
        if (!buttonWasPressed && buttonPressed)
            ArduinoManager.Instance.Translator.SetServo(1);
        buttonWasPressed = buttonPressed;
    }

    public void ReleaseTension()
    {
        StartCoroutine(ReleaseTensionCoroutine());
    }

    private IEnumerator ReleaseTensionCoroutine()
    {
        ArduinoManager.Instance.Translator.SetServo(1);

        yield return null;

        ArduinoManager.Instance.Translator.SetServo(0);
    }

    private void OnEncoderIncreased()
    {
        SetDebugText("Rotary encoder increased!");
    }

    private void SetDebugText(string text)
    {
        _curTextHideTime = _textHideTime;
        
        _debugText.text = text;
    }

    private void DebugTextTimer()
    {
        _curTextHideTime -= Time.deltaTime;

        if (_curTextHideTime <= 0)
        {
            _debugText.text = _textDefault;
        }
    }
}
