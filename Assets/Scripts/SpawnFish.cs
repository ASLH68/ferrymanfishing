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

    [SerializeField] private bool CaughtFish1;
    [SerializeField] private bool CaughtFish2;
    [SerializeField] private bool CaughtFish3;
    [SerializeField] private GameObject Fish1Spot;
    [SerializeField] private GameObject Fish2Spot;
    [SerializeField] private GameObject Fish3Spot;

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

    public void UIFish(float animateTime)
    {

        if (CaughtFish1 == false && CaughtFish2 == false && CaughtFish3 == false)
        {
            StartCoroutine(MoveFish(Fish1Spot, animateTime));
            CaughtFish1 = true;
            return;
        }
        if (CaughtFish1 == true && CaughtFish2 == false && CaughtFish3 == false)
        {
            StartCoroutine(MoveFish(Fish2Spot, animateTime));
            CaughtFish2 = true;
            return;
        }
        if (CaughtFish1 == true && CaughtFish2 == true && CaughtFish3 == false)
        {
            StartCoroutine(MoveFish(Fish3Spot, animateTime));
            CaughtFish3 = true;
            return;
        }


    }

    public void ResetFish()
    { 
        CaughtFish1 = false;
        CaughtFish2 = false;
        CaughtFish3 = false;
        Fish1Spot.SetActive(false);
        Fish2Spot.SetActive(false); 
        Fish3Spot.SetActive(false);
    }
    private IEnumerator MoveFish(GameObject targetPosition, float animationTime)
    {
        Vector3 Start = _fishImageObject.transform.position;
        float time = 0;
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
    }

}
