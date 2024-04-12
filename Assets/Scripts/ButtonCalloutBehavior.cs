using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct Callout
{
    public string Name;
    public Sprite Image;
    public AudioClip Sound;
}

public class ButtonCalloutBehavior : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Callout> _callouts;
    private int _callIndex;

    private PlayerInput _myPlayerInput;
    private InputAction _almost, _slowDown, _speedUp, _keepGoing, _steady, _pause;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _myPlayerInput = GetComponent<PlayerInput>();
        _myPlayerInput.currentActionMap.Enable();

        _almost = _myPlayerInput.currentActionMap.FindAction("Almost");
        _slowDown = _myPlayerInput.currentActionMap.FindAction("SlowDown");
        _speedUp = _myPlayerInput.currentActionMap.FindAction("SpeedUp");
        _keepGoing = _myPlayerInput.currentActionMap.FindAction("KeepGoing");
        _steady = _myPlayerInput.currentActionMap.FindAction("Steady");
        _pause = _myPlayerInput.currentActionMap.FindAction("Pause");

        _almost.started += _ => _callIndex = 0;
        _slowDown.started += _ => _callIndex = 1;
        _speedUp.started += _ => _callIndex = 2;
        _keepGoing.started += _ => _callIndex = 3;
        _steady.started += _ => _callIndex = 4;
        _pause.started += _ => _callIndex = 5;

        _almost.started += InvokeCallout;
        _slowDown.started += InvokeCallout;
        _speedUp.started += InvokeCallout;
        _keepGoing.started += InvokeCallout;
        _steady.started += InvokeCallout;
        _pause.started += InvokeCallout;

        //_myPlayerInput.currentActionMap.actionTriggered += InvokeCallout; 
            // triggered 3 times for started, performed, and canceled
    }

    /// <summary>
    /// Function called by the instruction keybinds. This function is used by
    /// every callout to change the sprite, play the animation, and play the
    /// sound associated with each callout in the struct based on _callIndex. 
    /// </summary>
    private void InvokeCallout(InputAction.CallbackContext obj)
    {
        _spriteRenderer.sprite = _callouts[_callIndex].Image;
        _anim.SetTrigger("callout");
        AudioSource.PlayClipAtPoint(_callouts[_callIndex].Sound, transform.position);
    }

    private void OnDisable()
    {
        _almost.started -= _ => _callIndex = 0;
        _slowDown.started -= _ => _callIndex = 1;
        _speedUp.started -= _ => _callIndex = 2;
        _keepGoing.started -= _ => _callIndex = 3;
        _steady.started -= _ => _callIndex = 4;
        _pause.started -= _ => _callIndex = 5;

        _almost.started -= InvokeCallout;
        _slowDown.started -= InvokeCallout;
        _speedUp.started -= InvokeCallout;
        _keepGoing.started -= InvokeCallout;
        _steady.started -= InvokeCallout;
        _pause.started -= InvokeCallout;
    }

}
