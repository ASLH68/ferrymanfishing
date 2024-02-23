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

    private void Start()
    {
        ArduinoManager.Instance.Translator.OnRotaryEncoderIncreased += OnEncoderIncreased;

        _debugText.text = _textDefault;
    }

    private void Update()
    {
        if (ArduinoManager.Instance.Translator.ButtonPressed)
        {
            SetDebugText("Button pressed!");
        }

        DebugTextTimer();
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
