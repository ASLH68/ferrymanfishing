using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IntroSceneBehavior : MonoBehaviour
{
    private Image _introImage;
    private Image _gentleImage;
    public static IntroSceneBehavior Instance;
    [SerializeField] private GameObject _castScreen;
    [SerializeField] private GameObject _gentleScreen;
    
    [SerializeField] private float _gentleScreenDur;

    [Header("BG UI")]
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private GameObject _fishBowl;
    /// <summary>
    /// if the casting screen is still acitve it will stay but if cast is pressed then the casting screen should 
    /// disappear
    /// </summary>
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
    /// <summary>
    /// At the start of the game this sets the cast screen to being true and it gets the intro scene image
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        _castScreen.gameObject.SetActive(true);
        _introImage = transform.GetChild(0).GetComponent<Image>();
        _gentleImage = transform.GetChild(1).GetComponent<Image>();
        _gentleImage = transform.GetChild(1).GetComponent<Image>();
        _progressBar.SetActive(false);
        _fishBowl.SetActive(false);
    }

    /// <summary>
    /// this to set and call CastingScreen in the Gameskeleton to make intro scene fadeout 
    /// </summary>
    /// <param name="val"></param>
    public void CastingScreen()
    {
        if (GameController.Instance.NumFishCaught == 0)
        {
            StartFading();
        }
    }

    /// <summary>
    /// This makes the Intro Scene Fadeout Once it gets called
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut(Image img)
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = img.color;
            c.a = f;
            img.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        img.gameObject.SetActive(false);

        if (img.Equals(_introImage))
        {
            StartCoroutine(FadeInGentleImg());
        } 
        else if (img.Equals(_gentleImage))
        {
            GameSkeleton.Instance.TriggerCast();
        }
    }

    /// <summary>
    /// This coroutine calls for FadeOut
    /// </summary>
    public void StartFading()
    {
        StartCoroutine(FadeOut(_introImage));
    }

    /// <summary>
    /// Fades in the be gentle img
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInGentleImg()
    {
        for (float f = 0f; f < 1.5f; f += 0.05f)
        {
            Color c = _gentleImage.color;
            c.a = f;
            _gentleImage.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        _progressBar.SetActive(true);
        _fishBowl.SetActive(true);
        yield return new WaitForSeconds(_gentleScreenDur);
        StartCoroutine(FadeOut(_gentleImage));
    }
}
