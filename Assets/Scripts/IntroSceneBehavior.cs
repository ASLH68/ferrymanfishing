using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IntroSceneBehavior : MonoBehaviour
{
    public static IntroSceneBehavior Instance;
    [SerializeField]private GameObject _castScreen;
    /// <summary>
    /// if the casting screen is still acitve it will stay but if cast is pressed then the casting screen should 
    /// disappear
    /// </summary>
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    /// <summary>
    /// At the start of the game this sets the cast screen to being true 
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        _castScreen.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// this to set and call CastingScreen in the Gameskeleton to set the cast screen to false 
    /// </summary>
    /// <param name="val"></param>
    public void CastingScreen(bool val)
    {
        _castScreen.gameObject.SetActive(val);
    }
}
