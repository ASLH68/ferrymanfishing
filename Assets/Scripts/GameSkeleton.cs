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
    private InputAction _reel, _cast, _lock, _unlock;
    public static int TotalFishCaught;
    [SerializeField] private Animator _anim;

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
    /// <summary>
    /// When Cast input is detected, this function determines how long the
    /// player will wait for the reeling to start, and starts the cast timer 
    /// with that random time.
    /// </summary>
    public void WhenCast(InputAction.CallbackContext obj)
    {
        if (_canCast)
        {
            _anim.SetTrigger("Cast");
            StartCoroutine(CastTimer());
            IntroSceneBehavior.Instance.CastingScreen(false);
            UIController.Instance.CastText(false);
        }
    }

    public void WhenCast()
    {
        if (_canCast)
        {
            _anim.SetTrigger("Cast");
            _castWaitTime = UnityEngine.Random.Range(_castWaitTimeMin, _castWaitTimeMax + 1);
            StartCoroutine(CastTimer());
            IntroSceneBehavior.Instance.CastingScreen(false);
            UIController.Instance.CastText(false);

            LockServo();
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
        _anim.SetTrigger("Captured");

        //if timer is null start one and cache it
        if (_reelTimer == null)
        {
            _reelTimer = StartCoroutine(ReelTimer());
        }

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
    private void ReelCount(InputAction.CallbackContext obj)
    {
        if (_canReel)
        {
            ReelValue += _reelIncrementValue;
            _anim.SetTrigger("Reel");

            //chack if milestone was reached
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
        _anim.SetTrigger("Success");
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
        switch (_milestonesReached)
        {
            case 0:
                _currentReelMilestone = _milestone1; //1.5f;
                if (_usingArduino)
                {
                    StartCoroutine(ControlRumble(_castRumble));
                }
                break;
            case 1:
                _currentReelMilestone = _milestone2; // 2.0f;
                if (_usingArduino)
                {
                    StartCoroutine(ControlRumble(RandomRumble()));
                    ServoCatch();
                }
                break;
            case 2:
                _currentReelMilestone = _milestone3; // 2.5f;
                if (_usingArduino)
                {
                    StartCoroutine(ControlRumble(RandomRumble()));
                    ServoCatch();
                }
                break;
            case 3:     //the fish is caught
                print("YOU CAUGHT THE FISH!! (reeling)");
                _anim.SetTrigger("Success");
                _milestonesReached = -1;
                StopCoroutine(_reelTimer);
                UIController.Instance.ReelText(false);
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
    /// Called in an animatin event in the fishing rod's success animation
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
        print("displaying fish and animation now");

        UIController.Instance.CatchingText(true);

        _reelTimer = null;
        _canReel = false;

        //DisplayFish
        GameController.Instance.ChooseFish();

        yield return new WaitForSeconds(_displayFishTime);

        yield return new WaitForSeconds(_fishToCerberusTime);

        TotalFishCaught++;
        if (TotalFishCaught == 3)
        {
            //end game
            EndGame.LoadEndScene();
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
            ArduinoManager.Instance.Translator.SetServo(0);
            ArduinoManager.Instance.Translator.SetServo(1);
        }
    }

    private void OnDisable()
    {
        if(_usingArduino)
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
    }
}
