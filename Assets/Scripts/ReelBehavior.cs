using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReelBehavior : MonoBehaviour
{
    public static float ReelValue;
    private float _reelMilestone = 2.0f;

    private PlayerInput _myPlayerInput;
    private InputAction _reel;

    void Start()
    {
        _myPlayerInput = GetComponent<PlayerInput>();
        _myPlayerInput.currentActionMap.Enable();
        _reel = _myPlayerInput.currentActionMap.FindAction("Reel");
        _reel.started += ReelStarted;
    }

    private void ReelStarted(InputAction.CallbackContext obj)
    {
        ReelValue += 0.1f;
        print(ReelValue);
        if(ReelValue >= _reelMilestone)
        {
            print("milestone reached");
            ReelValue = 0;
            //update next milestone value;
        }
    }

    void Update()
    {
        
    }
}
