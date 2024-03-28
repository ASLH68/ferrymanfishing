using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CastScreen : MonoBehaviour
{
    public static CastScreen Instance;

    private Image _castScreen;
    private Color _transparentCol;

    public Action AShowScreen;
    public Action AHideScreen;

    private void OnEnable()
    {
        AShowScreen += CallShowScreen;
        AHideScreen += CallHideScreen;
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

    // Start is called before the first frame update
    private void Start()
    {
        _castScreen = GetComponentInChildren<Image>();
        _transparentCol = new Color(_castScreen.color.r, _castScreen.color.g, _castScreen.color.b, 0);
        _castScreen.color = _transparentCol;
        _castScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// Calls the show screen coroutine
    /// </summary>
    private void CallShowScreen()
    {
        StartCoroutine(ShowScreen());
    }

    /// <summary>
    /// Calls the hide screen coroutine
    /// </summary>
    private void CallHideScreen()
    {
        StartCoroutine(HideScreen());

    }

    /// <summary>
    /// Fades the screen in
    /// </summary>
    private IEnumerator ShowScreen()
    {
        _castScreen.gameObject.SetActive(true);
        _castScreen.color = _transparentCol;
        for (float f = 0f; f <= 1f; f += 0.05f)
        {
            Color c = _castScreen.color;
            c.a = f;
            _castScreen.color = c;
            yield return new WaitForSeconds(0.05f);
        }

    }

    /// <summary>
    /// Fades the screen out
    /// </summary>
    private IEnumerator HideScreen()
    {
        for (float f = 1f; f >= 0f; f -= 0.1f)
        {
            Color c = _castScreen.color;
            c.a = f;
            _castScreen.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        _castScreen.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        AShowScreen -= CallShowScreen;
        AHideScreen -= CallHideScreen;
    }

}