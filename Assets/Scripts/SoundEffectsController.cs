using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using UnityEngine;

public class SoundEffectsController : MonoBehaviour
{
    public static SoundEffectsController Instance;
    public List<AudioClip> _commonFishSounds;
    public List<AudioClip> _legendaryFishSounds;

    public GameObject _audiocontroller;

    public AudioClip sound;
    public AudioSource source;
    private AudioClip _iceCreamFish;
    private AudioClip _coinFish;
    private AudioClip _vaseFish;
    private AudioClip _charlotteFish;
    private AudioClip _sadFish;
    private AudioClip _skullRay;
    private AudioClip _whirlpoolFish;
    private AudioClip _krocto;
    private AudioClip _skullFish;

    private bool _caughtFish1;
    private bool _caughtFish2;
    private bool _caughtFish3;

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
    /// when fish gets caught and shows up on screen the sound effect that is for the same fish will be played
    /// </summary>
    public void FishSound()
    {
        if (SpawnFish.Instance.Caughtfish1() == false && _caughtFish2 == false && _caughtFish3 == true)
        {
            int legendaryFishSFX = SpawnFish.Instance.FishIndex();
            Debug.Log(legendaryFishSFX);
            source.clip = _legendaryFishSounds.ElementAt(legendaryFishSFX);
           source.Play();
            _legendaryFishSounds.Remove(_legendaryFishSounds[legendaryFishSFX]);
           
        }
        else
        {
            int commonFishSFX = SpawnFish.Instance.FishIndex();
            Debug.Log(commonFishSFX);
            source.clip = _commonFishSounds.ElementAt(commonFishSFX);
            source.Play();
            _commonFishSounds.Remove(_commonFishSounds[commonFishSFX]);
        }
    }
   
    void Start()
    {
      
    }

    void Update()
    {
        
    }
}
