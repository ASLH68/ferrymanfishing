/*****************************************************************************
// File Name :         GameController
// Author :            Andrea Swihart-DeCoster
// Creation Date :     February 24th,2024
//
// Brief Description : Controls the flow of gameplay fish
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    // An instance of the  fishScriptableObject
    [SerializeField] private List<Fish> _commonFish;
    [SerializeField] private List<Fish> _legendaryFish;

    public int NumFishCaught { get; private set; }

    public Fish CurrentFish { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        NumFishCaught = 0;
    }


    /// <summary>
    /// Chooses a random fish out of the list
    /// </summary>
    /// <param name="fishList"></param>
    public void ChooseFish()
    {
        if(NumFishCaught < 2)
        {
            int fishIndex = Random.Range(0, _commonFish.Count);
            CurrentFish = _commonFish[fishIndex];
            _commonFish.Remove(CurrentFish);
        }
        else
        {
            int fishIndex = Random.Range(0, _legendaryFish.Count);
            CurrentFish = _legendaryFish[fishIndex];
            _legendaryFish.Remove(CurrentFish);
        }
        NumFishCaught++;

        FishUIDisplays.Instance.DisplayFish();
    }

    public bool IsLegendary()
    {
        return NumFishCaught == 3;
    }
}
