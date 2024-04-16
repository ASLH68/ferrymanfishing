/*****************************************************************************
// File Name :         SpawnFish
// Author :            Yael E. Martoral, Andrea Swihart-DeCoster
// Creation Date :     February 2,2024
//
// Brief Description : It spawns the fish after it was catched
*****************************************************************************/

using System.Windows.Forms.DataVisualization.Charting;
using UnityEngine;
using UnityEngine.UI;

public class FishUIDisplays : MonoBehaviour
{
    public static FishUIDisplays Instance;
    [SerializeField] private GameObject _fishImageObject;
     
    [Header("Fish Bowl")]
    [SerializeField] private GameObject _fish1Spot;
    [SerializeField] private GameObject _fish2Spot;
    [SerializeField] private GameObject _fish3Spot;

    [Header("Caught Background")]
    [SerializeField] private Image _caughtBGImage;
    [SerializeField] private Sprite _caught1Sprite;
    [SerializeField] private Sprite _caught2Sprite;
    [SerializeField] private Sprite _caught3Sprite;
    [SerializeField] private Image _fishCardImg;


    /// <summary>
    /// It checks if the Fish is on screen or not
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
    /// It chooses a fish at random out of the random pull
    /// </summary>
    private void SetCaughtImg()
    {
        BackgroundMusic.Instance.FadeOut();
        ChangeCaughtBG();
        _fishImageObject.GetComponentInChildren<Image>().sprite = GameController.Instance.CurrentFish.CaughtSprite;
        _fishImageObject.SetActive(true);          
    }

    /// <summary>
    /// Sets the img of the fish in the fishbowl UI
    /// </summary>
    public void SetFishBowlImg()
    {
        if (GameController.Instance.IsLegendary())
        {
            EndSceneBehavior.Instance.SetFishPng(3);

            _fish3Spot.GetComponent<Image>().sprite = GameController.Instance.CurrentFish.BowlSprite;
            _fish3Spot.SetActive(true);
        }
        else if (GameController.Instance.NumFishCaught == 1)
        {
            _fish1Spot.GetComponent<Image>().sprite = GameController.Instance.CurrentFish.BowlSprite;
            _fish1Spot.SetActive(true);

            EndSceneBehavior.Instance.SetFishPng(1);
        }
        else if (GameController.Instance.NumFishCaught == 2)
        {
            _fish2Spot.GetComponent<Image>().sprite = GameController.Instance.CurrentFish.BowlSprite;
            _fish2Spot.SetActive(true);

            EndSceneBehavior.Instance.SetFishPng(2);
        }
    }

    /// <summary>
    /// Sets the fish card img
    /// </summary>
    private void SetFishCardImg()
    {
        _fishCardImg.sprite = GameController.Instance.CurrentFish.FishNameCard;
    }

    /// <summary>
    /// Changes the background of the caught fish
    /// </summary>
    private void ChangeCaughtBG()
    {
        switch(GameController.Instance.NumFishCaught)
        {
            case 1:
                _caughtBGImage.sprite = _caught1Sprite;
                break;
            case 2:
                _caughtBGImage.sprite = _caught2Sprite;
                break;
            case 3:
                _caughtBGImage.sprite = _caught3Sprite;
                break;
        }
    }

    /// <summary>
    /// Removes the images from the top tight of the screen, not currently being used
    /// </summary>
    public void ResetFish()
    {
        _fish1Spot.SetActive(false);
        _fish2Spot.SetActive(false); 
        _fish3Spot.SetActive(false);
    }

    /// <summary>
    /// Displays the fish in the middle of the screen, plays the caught 
    /// animation, and adds the fish to the fishbowl
    /// </summary>
    public void DisplayFish()
    {
        SetCaughtImg();
        //SetFishBowlImg();
        SetFishCardImg();
        StartCoroutine(UIAnimController.Instance.PlayCaughtAnims());
    }
}
