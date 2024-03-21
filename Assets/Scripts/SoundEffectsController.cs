using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using UnityEngine;

public class SoundEffectsController : MonoBehaviour
{
    public static SoundEffectsController Instance;
    [SerializeField] private List<AudioClip> _commonFishSounds;
    [SerializeField] private List<AudioClip> _legendaryFishSounds;

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
        if (SpawnFish.Instance.Caughtfish1() && SpawnFish.Instance.Caughtfish2())
        {
            int legendaryFishSFX = SpawnFish.Instance.FishIndex();
            //Debug.Log(legendaryFishSFX);
            _source.clip = _legendaryFishSounds.ElementAt(legendaryFishSFX);
            _source.Play();
            _legendaryFishSounds.Remove(_legendaryFishSounds[legendaryFishSFX]);

        }
        else
        {
            int commonFishSFX = SpawnFish.Instance.FishIndex();
            //Debug.Log(commonFishSFX);
            _source.clip = _commonFishSounds.ElementAt(commonFishSFX);
            _source.Play();
            _commonFishSounds.Remove(_commonFishSounds[commonFishSFX]);
        }
    }
}
