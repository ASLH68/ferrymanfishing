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
    private Coroutine _currentTimer;
    private bool _canCast;

    private PlayerInput _myPlayerInput;
    private InputAction _cast;
    public static Action Cast;

    [SerializeField] private GameObject _castMessage;
    [SerializeField] private GameObject _victoryMessage;

    /// <summary>
    /// Begins the game and sets up inputs.
    /// </summary>
    private void Start()
    {
        SwitchFish();

        _myPlayerInput = GetComponent<PlayerInput>();
        _myPlayerInput.currentActionMap.Enable();
        _cast = _myPlayerInput.currentActionMap.FindAction("Cast");
        _cast.started += OnCast;
    }

    /// <summary>
    /// Assigns the Cast action
    /// </summary>
    private void OnEnable()
    {
        Cast += FishCaught;
    }

    /// <summary>
    /// Invokes the Cast action on the cast input.
    /// </summary>
    /// <param name="obj"></param>
    private void OnCast(InputAction.CallbackContext obj)
    {
        if(_canCast)
        {
            Cast?.Invoke();
        }
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
        StartCoroutine(DisplayFish());
    }

    /// <summary>
    /// Selects a random index of the Fish List.
    /// </summary>
    private void RandomFish()
    {
        _randomInt = UnityEngine.Random.Range(0, fishList.Count - 1);
    }

    /// <summary>
    /// Waits 2.5 seconds to display the fish, then begins the catching fish
    /// functionality.
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplayFish()
    {
        yield return new WaitForSeconds(2.5f);
        //listen for cast
        _canCast = true;
        _castMessage.SetActive(true);
        _currentTimer = StartCoroutine(CatchingFish(2.5f));
    }

    /// <summary>
    /// Waits a given amout of seconds before the fish is "caught". Will be 
    /// interrupted by the catch input functionality.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator CatchingFish(float time)
    {
        yield return new WaitForSeconds(time);
        FishCaught();
    }

    /// <summary>
    /// Stops the catching fish timer and resets the cast message. Increments
    /// TotalFishCaught and cexks if the game should end, if not starts the
    /// next round.
    /// </summary>
    private void FishCaught()
    {
        if(_currentTimer != null)
        {
            StopCoroutine(_currentTimer);
        }
        _castMessage.SetActive(false);
        //stop listening
        _canCast = false;

        TotalFishCaught++;
        if(TotalFishCaught == 3)
        {
            EndGame();
        }
        else
        {
            SwitchFish();
        }
    }

    /// <summary>
    /// Disables the current fish and enables the victory mesage.
    /// </summary>
    private void EndGame()
    {
        if (EnabledFish != null)
        {
            EnabledFish.SetActive(false);
        }

        _victoryMessage.SetActive(true);
    }

    /// <summary>
    /// Unassigns Cast action.
    /// </summary>
    private void OnDisable()
    {
        Cast -= FishCaught;
    }
}
