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
using UnityEngine.Windows;

public class GameSkeleton : MonoBehaviour
{
    public static GameSkeleton Instance;
    private PlayerInput _myPlayerInput;
    private InputAction _reel, _cast, _lock, _unlock, _quit, _goToEnd;
    public static int TotalFishCaught;
    private Animator _anim;

    [SerializeField] private int _numFishToCatch;

    [Header("Arduino")]
    [SerializeField] private bool _usingArduino;

    [Header("Cast Phase")]
    [SerializeField] private float _castWaitTimeMax;
    [SerializeField] private float _castWaitTimeMin;
    private float _castWaitTime;
    private bool _canCast;

    [Header("Reel Phase")]
    [SerializeField] private float _reelIncrementValue;
    public static float ReelValue;
    private int _milestonesReached;
    [HideInInspector]public bool _canReel = false;
    [SerializeField] private float _reelMaxTime;
    private Coroutine _reelTimer;
    [SerializeField] private float _reelAnimCooldown;
    private Coroutine _reelAnimCache;


    [Header("Display Fish Phase")]
    [SerializeField] private float _displayFishTime;
    [SerializeField] private float _fishToCerberusTime;


    [Header("Fish Milestones")]
    private float _currentReelMilestone;
    [SerializeField] private float _milestone1_1;
    [SerializeField] private float _milestone1_2;
    [SerializeField] private float _milestone1_3;
    [SerializeField] private float _milestone2_1;
    [SerializeField] private float _milestone2_2;
    [SerializeField] private float _milestone2_3;
    [SerializeField] private float _milestone3_1;
    [SerializeField] private float _milestone3_2;
    [SerializeField] private float _milestone3_3;
    private float servoCatchTime;
    [SerializeField] private float servoCatchTime1_1;
    [SerializeField] private float servoCatchTime1_2;
    [SerializeField] private float servoCatchTime1_3;
    [SerializeField] private float servoCatchTime2_1;
    [SerializeField] private float servoCatchTime2_2;
    [SerializeField] private float servoCatchTime2_3;
    [SerializeField] private float servoCatchTime3_1;
    [SerializeField] private float servoCatchTime3_2;
    [SerializeField] private float servoCatchTime3_3;

    [Header("Rumble Times")]
    [SerializeField] private float _milestoneRumbleMin;
    [SerializeField] private float _milestoneRumbleMax;
    [SerializeField] private float _catchRumble;
    [SerializeField] private float _castRumble;

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
        _lock = _myPlayerInput.currentActionMap.FindAction("LockServo");
        _unlock = _myPlayerInput.currentActionMap.FindAction("UnlockServo");
        _quit = _myPlayerInput.currentActionMap.FindAction("Quit");
        _goToEnd = _myPlayerInput.currentActionMap.FindAction("GoToEndScreen");

        _quit.started += QuitBuild;
        _goToEnd.started += SkipToEndScreen;

        _anim = GetComponent<Animator>();

        if (_usingArduino)
        {
            //_arduioPrefab.SetActive(true);
            ArduinoManager.Instance.gameObject.SetActive(true);
            ArduinoManager.Instance.Translator.OnButtonPressed += WhenCast;
            ArduinoManager.Instance.Translator.OnRotaryEncoderIncreased += ReelCount;

            _lock.started += LockServo;
            _unlock.started += UnlockServo;
        }
        else
        {
            //ArduinoManager.Instance.gameObject.SetActive(false);
            _reel.started += ReelCount;
            _cast.started += WhenCast;
        }

        _canCast = true;
        print("cast");

        TotalFishCaught = 0;
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

    public void WhenCast()
    {
        if (_canCast)
        {
            _canCast = false;
            if(GameController.Instance.NumFishCaught >= 1)
            {
                TriggerCast();
            }
            else
            {
                IntroSceneBehavior.Instance.CastingScreen();
            }

            
            LockServo();
        }
    }

    /// <summary>
    /// When Cast input is detected, this function determines how long the
    /// player will wait for the reeling to start, and starts the cast timer 
    /// with that random time.
    /// </summary>
    public void WhenCast(InputAction.CallbackContext obj)
    {
        if (_canCast)
        {
            _canCast = false;
            if (GameController.Instance.NumFishCaught >= 1)
            {
                TriggerCast();
            }
            else
            {
                IntroSceneBehavior.Instance.CastingScreen();
            }
        }
    }

    public void TriggerCast()
    {
        //CastScreen.Instance.AHideScreen?.Invoke();
        _anim.SetTrigger("Cast");
        _castWaitTime = UnityEngine.Random.Range(_castWaitTimeMin, _castWaitTimeMax + 1);
        StartCoroutine(CastTimer());
    }

    /// <summary>
    /// Starts reel phase after the cast timer is up
    /// </summary>
    /// <returns></returns>
    private IEnumerator CastTimer()
    {
        yield return new WaitForSeconds(_castWaitTime);

        _anim.SetTrigger("Captured");
        _canReel = true;

        //if timer is null start one and cache it
        if (_reelTimer == null)
        {
            _reelTimer = StartCoroutine(ReelTimer());
        }

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
        if (_canReel)
        {
            ReelValue += _reelIncrementValue;

            _anim.SetTrigger("Reel");
            if (_reelAnimCache != null)
            {
                StopCoroutine(_reelAnimCache);
            }
            _reelAnimCache = StartCoroutine(ReelAnimCooldown());

            //check if milestone was reached
            if (ReelValue >= _currentReelMilestone)
            {
                print("milestone reached");
                ReelValue = 0;
                UpdateNextMilestone();
            }
        }
    }

    private void ReelCount()
    {
        if (_canReel)
        {
            ReelValue += _reelIncrementValue;

            _anim.SetTrigger("Reel");
            if (_reelAnimCache != null)
            {
                StopCoroutine(_reelAnimCache);
            }
            _reelAnimCache = StartCoroutine(ReelAnimCooldown());

            //check if milestone was reached
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
        _anim.SetTrigger("Success");
        _milestonesReached = 3;
        UpdateNextMilestone();
    }

    /// <summary>
    /// A coroutine to aid the reeling animation's responsiveness. Ends Reel anim.
    /// </summary>
    /// <returns></returns>
    IEnumerator ReelAnimCooldown()
    {
        yield return new WaitForSeconds(_reelAnimCooldown);
        _anim.SetTrigger("EndReel");
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
                UpdateCurrentValues();
                if (_usingArduino)
                {
                    StartCoroutine(ControlRumble(_castRumble));
                }
                break;
            case 1:
                UpdateCurrentValues();
                _anim.SetTrigger("MilestoneReel");
                if (_usingArduino)
                {
                    StartCoroutine(ControlRumble(RandomRumble()));
                    ServoCatch();
                }
                break;
            case 2:
                UpdateCurrentValues();
                _anim.SetTrigger("MilestoneReel");
                if (_usingArduino)
                {
                    StartCoroutine(ControlRumble(RandomRumble()));
                    ServoCatch();
                }
                break;
            case 3:     //the fish is caught
                print("YOU CAUGHT THE FISH!! (reeling)");
                UpdateCurrentValues();
                _anim.SetTrigger("Success");
                StopCoroutine(_reelTimer);
                _canReel = false;
                if (_usingArduino)
                {
                    StartCoroutine(ControlRumble(_catchRumble));
                    ServoCatch();
                }
                break;
        }
        _milestonesReached++;
    }

    /// <summary>
    /// This function updates the value of the current milestone and servo catch
    /// time depending on the number of fish caught and milestones reached.
    /// </summary>
    private void UpdateCurrentValues()
    {
        if(TotalFishCaught == 0) //first fish
        {
            switch(_milestonesReached)
            {
                case 0:
                    _currentReelMilestone = _milestone1_1;
                    servoCatchTime = 0.1f;
                    break;
                case 1:
                    _currentReelMilestone = _milestone1_2;
                    servoCatchTime = servoCatchTime1_1;
                    break;
                case 2:
                    _currentReelMilestone = _milestone1_3;
                    servoCatchTime = servoCatchTime1_2;
                    break;
                default:
                    _milestonesReached = -1;
                    servoCatchTime = servoCatchTime1_3;
                    break;
            }
        }
        else if(TotalFishCaught == 1) //second fish
        {
            switch (_milestonesReached)
            {
                case 0:
                    _currentReelMilestone = _milestone2_1;
                    servoCatchTime = 0.1f;
                    break;
                case 1:
                    _currentReelMilestone = _milestone2_2;
                    servoCatchTime = servoCatchTime2_1;
                    break;
                case 2:
                    _currentReelMilestone = _milestone2_3;
                    servoCatchTime = servoCatchTime2_2;
                    break;
                default:
                    _milestonesReached = -1;
                    servoCatchTime = servoCatchTime2_3;
                    break;
            }
        }
        else if(TotalFishCaught >= 2) //third and any following fish
        {
            switch (_milestonesReached)
            {
                case 0:
                    _currentReelMilestone = _milestone3_1;
                    servoCatchTime = 0.1f;
                    break;
                case 1:
                    _currentReelMilestone = _milestone3_2;
                    servoCatchTime = servoCatchTime3_1;
                    break;
                case 2:
                    _currentReelMilestone = _milestone3_3;
                    servoCatchTime = servoCatchTime3_2;
                    break;
                default:
                    _milestonesReached = -1;
                    servoCatchTime = servoCatchTime3_3;
                    break;
            }
        }

        print(_currentReelMilestone + " and " + servoCatchTime);
    }



    /// <summary>
    /// Called in an animation event in the fishing rod's success animation
    /// </summary>
    private void StartFishDisplayCoroutine()
    {
        StartCoroutine(FishDisplay());
    }

    /// <summary>
    /// Displays the caught fish for the player. Then determines if the game
    /// should loop and catch another fish or end.
    /// </summary>
    /// <returns></returns>
    IEnumerator FishDisplay()
    {
        _reelTimer = null;
        _canReel = false;

        //DisplayFish
        GameController.Instance.ChooseFish();

        yield return new WaitForSeconds(_displayFishTime);

        yield return new WaitForSeconds(_fishToCerberusTime);

        TotalFishCaught++;
        if (CaughtMaxFish())
        {
            EndSceneBehavior.Instance.GameOver();

        }
        else
        {
            //start at cast phase again
            print("cast");
            _canCast = true;
            //CastScreen.Instance.AShowScreen?.Invoke();
        }
    }

    public bool CaughtMaxFish()
    {
        return GameController.Instance.NumFishCaught == _numFishToCatch;
    }

    public float GetDisplayFishTime()
    {
        return _displayFishTime;
    }

    /// <summary>
    /// Coroutine to start and stop the controller rumble. Uses a float parameter
    /// to determine how long the rumble lasts.
    /// </summary>
    IEnumerator ControlRumble(float input)
    {
        if (_usingArduino)
        {
            ArduinoManager.Instance.Translator.ToggleHapticMotor(true);
            yield return new WaitForSeconds(input);
            ArduinoManager.Instance.Translator.ToggleHapticMotor(false);
        }
    }

    /// <summary>
    /// Randomly selects a length for the control to rumble for the milestones.
    /// </summary>
    private float RandomRumble()
    {
        return UnityEngine.Random.Range(_milestoneRumbleMin, _milestoneRumbleMax);
    }


    /// <summary>
    /// Locks the Servo for the controller
    /// </summary>
    private void LockServo(InputAction.CallbackContext obj)
    {
        if(_usingArduino)
        {
            ArduinoManager.Instance.Translator.SetServo(1);
        }
    }

    private void LockServo()
    {
        if (_usingArduino)
        {
            ArduinoManager.Instance.Translator.SetServo(1);
        }
    }

    /// <summary>
    /// Unlocks the Servo for the controller
    /// </summary>
    private void UnlockServo(InputAction.CallbackContext obj)
    {
        if (_usingArduino)
        {
            ArduinoManager.Instance.Translator.SetServo(0);
        }
    }

    /// <summary>
    /// unlocks and then locks the servo as fast as possible 
    /// </summary>
    private void ServoCatch()
    {
        if (_usingArduino)
        {
            StartCoroutine(ServoCatch1());
        }
    }

    IEnumerator ServoCatch1()
    {
        ArduinoManager.Instance.Translator.SetServo(0); //open
        yield return new WaitForSeconds(servoCatchTime);
        ArduinoManager.Instance.Translator.SetServo(1); //close
    }

    private void QuitBuild(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }

    private void SkipToEndScreen(InputAction.CallbackContext obj)
    {
        StopAllCoroutines();
        EndSceneBehavior.Instance.GameOver();
    }

    private void OnDisable()
    {
        if (_usingArduino)
        {
            ArduinoManager.Instance.Translator.OnButtonPressed -= WhenCast;
            ArduinoManager.Instance.Translator.OnRotaryEncoderIncreased -= ReelCount;
            _lock.started -= LockServo;
            _unlock.started -= UnlockServo;
        }
        else
        {
            _reel.started -= ReelCount;
            _cast.started -= WhenCast;
        }

        _quit.started -= QuitBuild;
        _goToEnd.started -= SkipToEndScreen;

    }
}
