using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEngine;

public class BackgroundMusicBehavior : MonoBehaviour
{
    public static BackgroundMusicBehavior Instance;

    public AudioSource _BGMusic;

    /// <summary>
    /// when a soundeffect plays for a fisf this'll set the background music volume to 0 and then to 1 once the 
    /// soundeffect has finished playing
    /// </summary>
    public void FadeOutMusic()
    {
        if (SoundEffectsController.Instance != null)
        {
            _BGMusic.volume = 0;
        }
        else
        {
            _BGMusic.volume = 1;
        }
    }

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

    void Start()
    {
       _BGMusic = GetComponent<AudioSource>(); 
    }

    void Update()
    {
        
    }
}
