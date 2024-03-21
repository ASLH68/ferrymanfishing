using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoFull : MonoBehaviour
{
    [SerializeField] private Text _debugText;
    [SerializeField] private string _textDefault = "Awaiting Data...";
    [SerializeField] [Range(0.1f, 5f)] private float _releaseTensionTime = 0.1f;
    [SerializeField] private float _textHideTime = 1f;
    private float _curTextHideTime;

    private void Start()
    {
        ArduinoManager.Instance.Translator.OnRotaryEncoderIncreased += OnEncoderIncreased;
        ArduinoManager.Instance.Translator.OnButtonPressed += ReleaseTension;

        _debugText.text = _textDefault;
    }

    private void OnDisable()
    {
        ArduinoManager.Instance.Translator.OnRotaryEncoderIncreased -= OnEncoderIncreased;
        ArduinoManager.Instance.Translator.OnButtonPressed -= ReleaseTension;
    }

    private void Update()
    {
        DebugTextTimer();
    }

    public void ReleaseTension()
    {
        StartCoroutine(ReleaseTensionCoroutine());
        SetDebugText("Releasing Tension!");
    }

    private IEnumerator ReleaseTensionCoroutine()
    {
        ArduinoManager.Instance.Translator.SetServo(0);

        yield return new WaitForSeconds(_releaseTensionTime);

        ArduinoManager.Instance.Translator.SetServo(1);
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
