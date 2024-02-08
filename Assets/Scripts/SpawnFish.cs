/*****************************************************************************
// File Name :         SpawnFish
// Author :            Yael E. Martoral
// Creation Date :     February 2,2024
//
// Brief Description : It sapwns the fish after it was catched
*****************************************************************************/
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnFish : MonoBehaviour
{
    public static SpawnFish Instance;
    [SerializeField] private GameObject _fishImageObject;
    public List<Sprite> _fishSprites;

    [SerializeField] private bool _caughtFish1;
    [SerializeField] private bool _caughtFish2;
    [SerializeField] private bool _caughtFish3;
    [SerializeField] private GameObject _fish1Spot;
    [SerializeField] private GameObject _fish2Spot;
    [SerializeField] private GameObject _fish3Spot;

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
        _fishImageObject.SetActive(value);
        ChangingFish();
    }

    /// <summary>
    /// It chosses a fish at random out of the random pull
    /// </summary>
    private void ChangingFish()
    {
        int numberFish = Random.Range(0, _fishSprites.Count);
        _fishSprites.ElementAt(numberFish);
        _fishImageObject.GetComponentInChildren<UnityEngine.UI.Image>().sprite = _fishSprites[numberFish];
    }

    /// <summary>
    /// It iddentifies when a fish has been caught and put it on the right side of the screen
    /// </summary>
    /// <param name="animateTime"></param>
    public void UIFish(float animateTime)
    {

        if (_caughtFish1 == false && _caughtFish2 == false && _caughtFish3 == false)
        {
            StartCoroutine(MoveFish(_fish1Spot, animateTime));
            _caughtFish1 = true;
            return;
        }
        if (_caughtFish1 == true && _caughtFish2 == false && _caughtFish3 == false)
        {
            StartCoroutine(MoveFish(_fish2Spot, animateTime));
            _caughtFish2 = true;
            return;
        }
        if (_caughtFish1 == true && _caughtFish2 == true && _caughtFish3 == false)
        {
            StartCoroutine(MoveFish(_fish3Spot, animateTime));
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
    /// 
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="animationTime"></param>
    /// <returns></returns>
    private IEnumerator MoveFish(GameObject targetPosition, float animationTime)
    {
        yield return new WaitForSeconds(2f);
        Vector3 Start = _fishImageObject.transform.position;
        float time =0;
        while (time < animationTime)
        {
            
            time += Time.deltaTime;
            float t = time/ animationTime;
            Vector3 currentPosition = Vector3.Lerp(Start, targetPosition.transform.position, t);
            _fishImageObject.transform.position = currentPosition;

            yield return null;
        }
        _fishImageObject.transform.position = Start;

        targetPosition.SetActive(true);
        targetPosition.GetComponent<UnityEngine.UI.Image>().sprite = _fishImageObject.GetComponent<UnityEngine.UI.Image>().sprite;

        //GameSkeleton skeleton = GameObject.FindObjectOfType<GameSkeleton>();
        //skeleton._canReel = true;

        FishApperance(false);
    }

}
