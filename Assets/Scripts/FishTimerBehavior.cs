/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: November 19, 2023
*    Description: 
*    
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTimerBehavior : MonoBehaviour
{
    public List<GameObject> FishList = new List<GameObject>();
    public static GameObject enabledFish;
    private int randomInt;

    private void Start()
    {
        SwitchFish();
    }

    /// <summary>
    /// Function that is called when a fish animation finishes to switch to a 
    /// new animation.
    /// </summary>
    public void SwitchFish()
    {
        //disable current fish
        if(enabledFish != null)
        {
            print("not  null");
            enabledFish.SetActive(false);
        }

        //pick new fish that is different from the previous
        RandomFish();
        print(randomInt);
        while (enabledFish == FishList[randomInt])
        {
            RandomFish();
        }

        //enable new fish
        enabledFish = FishList[randomInt];
        enabledFish.SetActive(true);
    }

    /// <summary>
    /// Selects a random index of the Fish List.
    /// </summary>
    private void RandomFish()
    {
        randomInt = Random.Range(0, FishList.Count);
    }
}
