/******************************************************************
*    Author: Johnatan Vargas
*    Contributors: 
*    Date Created: 1/26/2024
*    Description: ui controller
*******************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public TMP_Text CastingText;
    public TMP_Text ReelingText;
    public TMP_Text CaughtFishText;

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
    // Start is called before the first frame update
    void Start()
    {
       


        CastingText.gameObject.SetActive(true);
        ReelingText.gameObject.SetActive(false);
        CaughtFishText.gameObject.SetActive(false);
    }

    public void CastText(bool value)
    {
        CastingText.gameObject.SetActive(value);
    }
  
    public void ReelText(bool value)
    {
        ReelingText.gameObject.SetActive(value);
    }

    public void CatchingText(bool value)
    {
        CaughtFishText.gameObject.SetActive(value);
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
