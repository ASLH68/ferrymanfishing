/******************************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: January 19, 2024
*    Description: This script manages the reel phase. It reads the reel input,
*    updates the milestones, and times the phase so it does not take too long.
*    
******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReelBehavior : MonoBehaviour
{
    public static float ReelValue;
    [SerializeField] private float _reelIncrementValue;
    private float _currentReelMilestone;
    private int _milestonesReached;
    private bool _canReel;
    [SerializeField] private float _reelMaxTime;

    private PlayerInput _myPlayerInput;
    private InputAction _reel;
    private Coroutine _reelTimer;

    [Header("Fish Milestones")]
    [SerializeField] private float _milestone1;
    [SerializeField] private float _milestone2;
    [SerializeField] private float _milestone3;



    void Start()
    {
        _myPlayerInput = GetComponent<PlayerInput>();
        _myPlayerInput.currentActionMap.Enable();
        _reel = _myPlayerInput.currentActionMap.FindAction("Reel");
        _reel.started += ReelCount;

        _canReel = true;
        UpdateNextMilestone();
    }

    /// <summary>
    /// When Reel input is detected, this function increments the reel value to
    /// simulate the rod controller, starts the reel timer coroutine, and then
    /// checks if a milestone was reached.
    /// </summary>
    /// <param name="obj"></param>
    private void ReelCount(InputAction.CallbackContext obj)
    {
        if(_canReel)
        {
            //if timer is null start one and cache it
            if (_reelTimer == null)
            {
                _reelTimer = StartCoroutine(ReelTimer());
            }

            ReelValue += _reelIncrementValue;
            print(ReelValue);

            //chack if milestone was reached
            if (ReelValue >= _currentReelMilestone)
            {
                print("milestone reached");
                ReelValue = 0;
                UpdateNextMilestone();
            }
        }
    }

    /// <summary>
    /// Times the reeling phase so it does not last too long.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReelTimer()
    {
        print("timer started");
        yield return new WaitForSeconds(_reelMaxTime);
        print("YOU CAUGHT THE FISH!! (timer)");
        _canReel = false;
    }

    /// <summary>
    /// sets the milestones for the reeling phase at start and when a new 
    /// milestone is reached.
    /// </summary>
    private void UpdateNextMilestone()
    {
        switch (_milestonesReached)
        {
            case 0:
                _currentReelMilestone = _milestone1; //1.5f;
                break;
            case 1:
                _currentReelMilestone = _milestone2; // 2.0f;
                break;
            case 2:
                _currentReelMilestone = _milestone3; // 2.5f;
                break;
            case 3:     //the fish is caught
                print("YOU CAUGHT THE FISH!! (reeling)");
                StopCoroutine(_reelTimer);
                _reelTimer = null;
                _canReel = false;
                break;
        }
        _milestonesReached++;
    }
}
