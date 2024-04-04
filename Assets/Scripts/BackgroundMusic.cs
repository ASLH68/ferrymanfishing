using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;
    private AudioSource _bgMusic;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _defaultVolume;

    private void Start()
    {
        _bgMusic = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// this will fadeout the music 
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(Fade(false, _bgMusic, 0f));
    }
    /// <summary>
    /// this will fadein the music
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine(Fade(true, _bgMusic, _defaultVolume));
    }
    /// <summary>
    /// this will fade in and out the music 
    /// </summary>
    /// <param name="fadein"></param>
    /// <param name="_BGMusic"></param>
    /// <param name="duration"></param>
    /// <param name="targetVolume"></param>
    /// <returns></returns>
    public IEnumerator Fade(bool fadein, AudioSource _BGMusic, float targetVolume)
    {
        //Debug.Log("sound");
        //if (!fadein)
        //{
           // double lengthofSource = (double)_BGMusic.clip.samples / _BGMusic.clip.frequency;
         //   yield return new WaitForSecondsRealtime((float)(lengthofSource - duration));
       // }

        float time = 0f;
        float startVol = _BGMusic.volume;
        while (time < _fadeDuration)
        {
            string fadeSituation = fadein ? "fadeIn" : "fadeOut";
            //Debug.Log(fadeSituation);
            time += Time.deltaTime;
            _BGMusic.volume = Mathf.Lerp(startVol, targetVolume, time / _fadeDuration);
            yield return null;
        }


        if (!fadein)
        {
            SoundEffectsController.Instance.FishSound();
        }
        yield break;
    }

}
