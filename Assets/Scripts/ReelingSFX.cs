using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEngine;

public class ReelingSFX : MonoBehaviour
{
    public static ReelingSFX Instance;
    private AudioSource _ReelingSFX;

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

    // Start is called before the first frame update
    void Start()
    {
        _ReelingSFX = GetComponent<AudioSource>();
    }
    /// <summary>
    /// will start to play the reelingSFX when the reeling phase starts
    /// </summary>
    public void StartReelSFX()
    {
        _ReelingSFX.Play();
    }
    /// <summary>
    /// will pause the reelingSFX once the player catches the fish
    /// </summary>
    public void PauseReelSFX()
    {
        _ReelingSFX.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
