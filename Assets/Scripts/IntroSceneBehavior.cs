using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IntroSceneBehavior : MonoBehaviour
{
    private Image _introImage;
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
    /// At the start of the game this sets the cast screen to being true and it gets the intro scene image
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        _castScreen.gameObject.SetActive(true);
        _introImage = GetComponentInChildren<Image>();
        
    }

  
    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// this to set and call CastingScreen in the Gameskeleton to make intro scene fadeout 
    /// </summary>
    /// <param name="val"></param>
    public void CastingScreen(bool val)
    {
        StartFading();
        
    }
    
    /// <summary>
    /// This makes the Intro Scene Fadeout Once it gets called
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = _introImage.color;
            c.a = f;
            _introImage.color= c;
            yield return new WaitForSeconds(0.05f);
        }
        _castScreen.gameObject.SetActive(false);
    }
    /// <summary>
    /// This coroutine calls for FadeOut
    /// </summary>
    public void StartFading()
    {
        StartCoroutine("FadeOut");
        
    }

}
