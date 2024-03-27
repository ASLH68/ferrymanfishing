using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneBehavior : MonoBehaviour
{
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _endingDuration;

    AsyncOperation _asyncOperation;
    private void Start()
    {
        StartCoroutine(FadeBackground(0, 1, 0.01f));
    }


    /// <summary>
    /// Fades the background in and out
    /// </summary>
    /// <param name="start">Initial alpha</param>
    /// <param name="end">ending alpha</param>
    /// <param name="change">alpha delta</param>
    /// <returns></returns>
    private IEnumerator FadeBackground(float start, float end, float change)
    {
        Image background = GetComponentInChildren<Image>();
        float alpha = start;

        while(Mathf.Abs(alpha - end) > 0.01)
        {
            alpha += change;
            
            background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
            Debug.Log(alpha);
            yield return new WaitForSeconds(_fadeDuration * 0.01f);
        }

        if(start == 0)
        {
            SceneManager.UnloadSceneAsync(0);
            StartCoroutine(ReturnToGame());
        }
        if(start == 1)
        {
            SceneManager.UnloadSceneAsync(1);
            _asyncOperation.allowSceneActivation = true;
            
        }
    }

    /// <summary>
    /// Loads in the game scene and fades out the background
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReturnToGame()
    {
        _asyncOperation = SceneManager.LoadSceneAsync(0);
        _asyncOperation.allowSceneActivation = false;

        yield return new WaitForSeconds(_endingDuration);        
        StartCoroutine(FadeBackground(1, 0, -0.01f));
    }
}
