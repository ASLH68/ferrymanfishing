using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodSFX : MonoBehaviour
{
    [SerializeField] private GameObject _woosh;
    [SerializeField] private GameObject _bobber;
    [SerializeField] private GameObject _shine;

    /// <summary>
    /// when this gets called the shine sfx will play alongside the shine effect on the hook
    /// </summary>
    public void PlayShine()
    {
        _shine.GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// when this gets called the bobber sfx will play alongside the animation of when the bobber hits the water
    /// </summary>
    public void PlayBobber()
    {
        _bobber.GetComponent<AudioSource>().Play();
    }
    /// <summary>
    /// when this gets called the woosh sfx will play alongside when the animation is whipping the rod 
    /// </summary>
    public void PlayWoosh()
    {
      _woosh.GetComponent<AudioSource>().Play();
    }
}
