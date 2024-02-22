/*****************************************************************************
// File Name :         SpawnFish
// Author :            Yael E. Martoral
// Creation Date :     February 2,2024
//
// Brief Description : It sapwns the fish after it was catched
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpawnFish : MonoBehaviour
{
    public static SpawnFish Instance;
    [SerializeField] private GameObject _fishImageObject;
    public List<Sprite> _fishSprites;
    public List<Sprite> _legendaryFS;

    private bool _caughtFish1;
    private bool _caughtFish2;
    private bool _caughtFish3;
    [SerializeField] private GameObject _fish1Spot;
    [SerializeField] private GameObject _fish2Spot;
    [SerializeField] private GameObject _fish3Spot;
    [SerializeField] private float _animationTime;
    [SerializeField] private Image _uICanvas;
    [SerializeField] private Sprite _fishCaughtImage1;
    [SerializeField] private Sprite _fishCaughtImage2;
    [SerializeField] private Sprite _fishCaughtImage3;

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
    /// It shows a fish on the midlde of the screen
    /// </summary>
    /// <param name="value"></param>
    public void FishApperance(bool value)
    {
        if (value == true)
        {
            ChangingFish();
        }
        _fishImageObject.SetActive(value);
        
    }

    /// <summary>
    /// It chosses a fish at random out of the random pull
    /// </summary>
    private void ChangingFish()
    {
        if (_caughtFish1 == true && _caughtFish2 == true && _caughtFish3 == false)
        {
            int numberFish = Random.Range(0, _legendaryFS.Count);
            _legendaryFS.ElementAt(numberFish);
            _fishImageObject.GetComponentInChildren<Image>().sprite = _legendaryFS[numberFish];
            _legendaryFS.Remove(_legendaryFS[numberFish]);
        }
        else
        {
            int numberFish = Random.Range(0, _fishSprites.Count);
            _fishSprites.ElementAt(numberFish);
            _fishImageObject.GetComponentInChildren<UnityEngine.UI.Image>().sprite = _fishSprites[numberFish];
            _fishSprites.Remove(_fishSprites[numberFish]);
        }
    }

    /// <summary>
    /// It iddentifies when a fish has been caught and put it on the right side of the screen
    /// </summary>
    /// <param name="animateTime"></param>
    public void UIFish(float animateTime)
    {

        if (_caughtFish1 == false && _caughtFish2 == false && _caughtFish3 == false)
        {
            _uICanvas.sprite = _fishCaughtImage1;
            StartCoroutine(UIController.Instance.CaughtFishUI(_fish1Spot,_fishImageObject));
            //StartCoroutine(MoveFish(_fish1Spot, animateTime));
            _caughtFish1 = true;
            return;
        }
        if (_caughtFish1 == true && _caughtFish2 == false && _caughtFish3 == false)
        {
            _uICanvas.sprite = _fishCaughtImage2;
            StartCoroutine(UIController.Instance.CaughtFishUI(_fish2Spot, _fishImageObject));
            //StartCoroutine(MoveFish(_fish2Spot, animateTime));
            _caughtFish2 = true;
            return;
        }
        if (_caughtFish1 == true && _caughtFish2 == true && _caughtFish3 == false)
        {
            _uICanvas.sprite = _fishCaughtImage3;
            StartCoroutine(UIController.Instance.CaughtFishUI(_fish3Spot, _fishImageObject));
            //StartCoroutine(MoveFish(_fish3Spot, animateTime));
            _caughtFish3 = true;
            return;
        }
    }

    /// <summary>
    /// Removes the images from the top tight of the screen, not currently being used
    /// </summary>
    public void ResetFish()
    { 
        _caughtFish1 = false;
        _caughtFish2 = false;
        _caughtFish3 = false;
        _fish1Spot.SetActive(false);
        _fish2Spot.SetActive(false); 
        _fish3Spot.SetActive(false);
    }

    /// <summary>
    /// After waiting for Seconds, it moves the fish to one of the empty gameObjects on the scene
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="animationTime"></param>
    /// <returns></returns>
    //private IEnumerator MoveFish(GameObject targetPosition, float animationTime)
    //{
    //    yield return new WaitForSeconds(GameSkeleton.Instance.GetDisplayTime());
    //    Vector3 Start = _fishImageObject.transform.position;
    //    float time =0;
    //    while (time < animationTime)
    //    {
            
    //        time += Time.deltaTime;
    //        float t = time/ animationTime;
    //        Vector3 currentPosition = Vector3.Lerp(Start, targetPosition.transform.position, t);
    //        _fishImageObject.transform.position = currentPosition;

    //        yield return null;
    //    }
    //    _fishImageObject.transform.position = Start;

    //    targetPosition.SetActive(true);
    //    targetPosition.GetComponent<UnityEngine.UI.Image>().sprite = _fishImageObject.GetComponent<UnityEngine.UI.Image>().sprite;

    //    //GameSkeleton skeleton = GameObject.FindObjectOfType<GameSkeleton>();
    //    //skeleton._canReel = true;

    //    FishApperance(false);
    //}

    /// <summary>
    /// It calls both the FishApperance and UIFish function
    /// </summary>
    public void DisplayFish()
    {
        FishApperance(true);
        UIFish(_animationTime);

    }

}