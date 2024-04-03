using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAnimController : MonoBehaviour
{
    public static UIAnimController Instance;
    [SerializeField] private Animator _uiAnimator;
    [SerializeField] private ParticleSystem _Lsplash;
    [SerializeField] private ParticleSystem _Rsplash;

    public void Awake()
    {
        _Lsplash.Stop();
        _Rsplash.Stop();
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
    /// it calls in the animator to animate the UI of the fish caught
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayCaughtAnims()
    {
        _uiAnimator.Play("FadeIn");
        _Lsplash.Play();
        _Rsplash.Play();
        yield return new WaitForSeconds(GameSkeleton.Instance.GetDisplayFishTime());
        _Lsplash.Stop();
        _Rsplash.Stop();
        BackgroundMusic.Instance.FadeIn();
        if (!GameSkeleton.Instance.CaughtMaxFish())
        {
            _uiAnimator.Play("FadeOut");
            FishUIDisplays.Instance.SetFishBowlImg();
        }
    }
}
