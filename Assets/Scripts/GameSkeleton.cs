/******************************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: January 26, 2024
*    Description: This script manages all the phases in the game.
*    
******************************************************************************/
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSkeleton : MonoBehaviour
{
    public static GameSkeleton Instance;
    private PlayerInput _myPlayerInput;
    private InputAction _reel, _cast;
    public static int TotalFishCaught;

    [Header("Cast Phase")]
    [SerializeField] private float _castWaitTimeMax;
    [SerializeField] private float _castWaitTimeMin;
    private float _castWaitTime;
    private bool _canCast;

    [Header("Reel Phase")]
    [SerializeField] private float _reelIncrementValue;
    public static float ReelValue;
    private float _currentReelMilestone;
    private int _milestonesReached;
    [HideInInspector]public bool _canReel = false;
    [SerializeField] private float _reelMaxTime;
    private Coroutine _reelTimer;

 
    [Header("Display Fish Phase")]
    [SerializeField] private float _displayFishTime;
    [SerializeField] private float _fishToCerberusTime;


    [Header("Fish Milestones")]
    [SerializeField] private float _milestone1;
    [SerializeField] private float _milestone2;
    [SerializeField] private float _milestone3;

    /// <summary>
    /// Called before start
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    /// <summary>
    /// It sets values to the variables
    /// </summary>
    void Start()
    {
        _myPlayerInput = GetComponent<PlayerInput>();
        _myPlayerInput.currentActionMap.Enable();

        _reel = _myPlayerInput.currentActionMap.FindAction("Reel");
        _cast = _myPlayerInput.currentActionMap.FindAction("Cast");
        //_reel.started += ReelCount;
        //_cast.started += WhenCast;
        ArduinoManager.Instance.Translator.OnButtonPressed += WhenCast;
        ArduinoManager.Instance.Translator.OnRotaryEncoderIncreased += ReelCount;

        _canCast = true;
        print("cast");

       
    }
    #region getters
    /// <summary>
    /// It returns _displayFishTime
    /// </summary>
    /// <returns></returns>
    public float GetDisplayTime()
    {
        return _displayFishTime;
    }
    #endregion
    /// <summary>
    /// When Cast input is detected, this function determines how long the
    /// player will wait for the reeling to start, and starts the cast timer 
    /// with that random time.
    /// </summary>
    public void WhenCast() //;InputAction.CallbackContext obj)
    {
        if (_canCast)
        {
            _castWaitTime = UnityEngine.Random.Range(_castWaitTimeMin, _castWaitTimeMax+1);
            //print(_castWaitTime);
            StartCoroutine(CastTimer());
            IntroSceneBehavior.Instance.CastingScreen(false);
            UIController.Instance.CastText(false);
        }
    }

    /// <summary>
    /// Starts reel phase after the cast timer is up
    /// </summary>
    /// <returns></returns>
    private IEnumerator CastTimer()
    {
        _canCast = false;

        yield return new WaitForSeconds(_castWaitTime);

        //to start reel phase
        print("start reeling now");
        _canReel = true;
        UpdateNextMilestone();
        UIController.Instance.ReelText(true);
    }

    /// <summary>
    /// When Reel input is detected, this function increments the reel value to
    /// simulate the rod controller, starts the reel timer coroutine, and then
    /// checks if a milestone was reached.
    /// </summary>
    /// <param name="obj"></param>
    private void ReelCount() //InputAction.CallbackContext obj)
    {
        if (_canReel)
        {
            //if timer is null start one and cache it
            if (_reelTimer == null)
            {
                _reelTimer = StartCoroutine(ReelTimer());
            }

            ReelValue += _reelIncrementValue;
            //print(ReelValue);

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
        yield return new WaitForSeconds(_reelMaxTime);
        print("YOU CAUGHT THE FISH!! (timer)");
        _canReel = false;
        _milestonesReached = 3;
        UpdateNextMilestone();
        UIController.Instance.ReelText(false);
        UIController.Instance.CatchingText(true);
    }

    /// <summary>
    /// sets the milestones for the reeling phase at start and when a new 
    /// milestone is reached.
    /// </summary>
    private void UpdateNextMilestone()
    {
        StartCoroutine(ControlRumble(1f));
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
                _milestonesReached = -1;
                StopCoroutine(_reelTimer);
                StartCoroutine(FishDisplay());
                UIController.Instance.ReelText(false);
                UIController.Instance.CatchingText(true);
                break;
        }
        _milestonesReached++;
    }

    /// <summary>
    /// Displays the caught fish for the player. Then determines if the game
    /// should loop and catch another fish or end.
    /// </summary>
    /// <returns></returns>
    IEnumerator FishDisplay()
    {
        print("displaying fish and animation now");

        _reelTimer = null;
        _canReel = false;

        //DisplayFish
        SpawnFish.Instance.DisplayFish();

        yield return new WaitForSeconds(_displayFishTime);

        //Fish to Cerberus
        //SpawnFish.Instance.FishApperance(false);

        yield return new WaitForSeconds(_fishToCerberusTime);

        TotalFishCaught++;
        if (TotalFishCaught == 3)
        {
            //end game
            print("GameOver");
            SceneManager.LoadScene(0);
        }
        else
        {
            //start at cast phase again
            print("cast");
            _canCast = true;
            UIController.Instance.CastText(true);
            UIController.Instance.CatchingText(false);
        }
    }

    IEnumerator ControlRumble(float input)
    {
        ArduinoManager.Instance.Translator.ToggleHapticMotor(true);
        yield return new WaitForSeconds(input);
        ArduinoManager.Instance.Translator.ToggleHapticMotor(false);
    }

    private void OnDisable()
    {

        //_reel.started -= ReelCount;
        //_cast.started -= WhenCast;
        ArduinoManager.Instance.Translator.OnButtonPressed -= WhenCast;
        ArduinoManager.Instance.Translator.OnRotaryEncoderIncreased -= ReelCount;
    }
}
