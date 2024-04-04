using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;
    public AudioSource _BGMusic;

    private void Start()
    {
        _BGMusic = GetComponent<AudioSource>();
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
    private void Update()
    {

    }
    /// <summary>
    /// this will fadeout the music when the corourtine starts
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(Fade(false, _BGMusic, 0.5f, 0f));
    }
    /// <summary>
    /// this will fadein the music when the coroutine starts
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine(Fade(true, _BGMusic, 1f, 1f));
    }
    /// <summary>
    /// this will fade in and out the music 
    /// </summary>
    /// <param name="fadein"></param>
    /// <param name="_BGMusic"></param>
    /// <param name="duration"></param>
    /// <param name="targetVolume"></param>
    /// <returns></returns>
    public IEnumerator Fade(bool fadein, AudioSource _BGMusic, float duration, float targetVolume)
    {
        Debug.Log("sound");
        //if (!fadein)
        //{
           // double lengthofSource = (double)_BGMusic.clip.samples / _BGMusic.clip.frequency;
         //   yield return new WaitForSecondsRealtime((float)(lengthofSource - duration));
       // }

        float time = 0f;
        float startVol = _BGMusic.volume;
        while (time < duration)
        {
            string fadeSituation = fadein ? "fadeIn" : "fadeOut";
            Debug.Log(fadeSituation);
            time += Time.deltaTime;
            _BGMusic.volume = Mathf.Lerp(startVol, targetVolume, time / duration);
            yield return null;
        }


        if (!fadein)
        {
            SoundEffectsController.Instance.FishSound();
        }
        yield break;
    }

}
