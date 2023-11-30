/******************************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: November 19, 2023
*    Description: This script manages the base time system that will be used
*    for the entire game.
*    
******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishTimerBehavior : MonoBehaviour
{
    [SerializeField] private List<GameObject> fishList = new List<GameObject>();
    public static GameObject EnabledFish;
    private int _randomInt;
    public static int TotalFishCaught;

    private PlayerInput _myPlayerInput;
    private InputAction cast;

    [SerializeField] private GameObject _castMessage;

    private void Start()
    {
        SwitchFish();

        _myPlayerInput = GetComponent<PlayerInput>();
        _myPlayerInput.currentActionMap.Enable();
        cast = _myPlayerInput.currentActionMap.FindAction("Cast");
        cast.started += OnCast;
    }

    /// <summary>
    // TODO
    /// </summary>
    /// <param name="obj"></param>
    private void OnCast(InputAction.CallbackContext obj)
    {
        print(Time.time);
    }

    /// <summary>
    /// Disables current fish object, selects a new one, enables it, and starts
    /// a timer.
    /// </summary>
    public void SwitchFish()
    {
        //disable current fish
        if(EnabledFish != null)
        {
            EnabledFish.SetActive(false);
        }

        //pick new fish that is different from the previous
        RandomFish();
        while (EnabledFish == fishList[_randomInt])
        {
            RandomFish();
        }

        //enable new fish
        EnabledFish = fishList[_randomInt];
        EnabledFish.SetActive(true);

        //start timer
        //TODO: start timer
    }

    /// <summary>
    /// Selects a random index of the Fish List.
    /// </summary>
    private void RandomFish()
    {
        _randomInt = UnityEngine.Random.Range(0, fishList.Count + 1);
        //print(_randomInt);
    }

    /// <summary>
    /// Increments TotalFishCaught and cexks if the game should end, if not
    /// starts the next round.
    /// </summary>
    private void FishCaught()
    {
        TotalFishCaught++;
        if(TotalFishCaught == 3)
        {
            //TODO: end game
        }
        else
        {
            SwitchFish();
        }
    }
}
