using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using UnityEngine;

public class SoundEffectsController : MonoBehaviour
{
    public static SoundEffectsController Instance;

    private AudioSource _source;

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
    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    /// <summary>
    /// when fish gets caught and shows up on screen the sound effect that is for the same fish will be played
    /// </summary>
    public void FishSound()
    {
        _source.clip = GameController.Instance.CurrentFish.CatchSound;
        _source.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        _source.clip = clip;
        _source.Play();
    }
}
