using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "ScriptableObjects/FishDataObject", order = 1)]
public class Fish : ScriptableObject
{
    public string FishName;

    [Tooltip("Image of the fish's name")]
    public Sprite FishNameCard;

    [Tooltip("SFX that plays when the fish is caught")]
    public AudioClip CatchSound;

    [Tooltip("Image that appears in the middle of the screen")]
    public Sprite CaughtSprite;

    [Tooltip("Image that appears in the fish bowl")]
    public Sprite BowlSprite;
        
}