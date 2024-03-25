using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAnimController : MonoBehaviour
{
    public static UIAnimController Instance;
    [SerializeField] private float _fadeDuration = 0.05f;
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
        yield return new WaitForSeconds(_fadeDuration);
        _Lsplash.Stop();
        _Rsplash.Stop();
        _uiAnimator.Play("FadeOut");
    }
}
