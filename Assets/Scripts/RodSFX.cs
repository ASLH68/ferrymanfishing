using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodSFX : MonoBehaviour
{
    [SerializeField] private GameObject Woosh;
    [SerializeField] private GameObject Bobber;
    [SerializeField] private GameObject Shine;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// when this gets called the shine sfx will play alongside the shine effect on the hook
    /// </summary>
    public void play_shine()
    {
        Shine.GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// when this gets called the bobber sfx will play alongside the animation of when the bobber hits the water
    /// </summary>
    public void play_bobber()
    {
        Bobber.GetComponent<AudioSource>().Play();
    }
    /// <summary>
    /// when this gets called the woosh sfx will play alongside when the animation is whipping the rod 
    /// </summary>
    public void play_woosh()
    {
      Woosh.GetComponent<AudioSource>().Play();
    }
}
