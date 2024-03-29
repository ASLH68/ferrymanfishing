using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneBehavior : MonoBehaviour
{
    public static EndSceneBehavior Instance;

    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _endingDuration;

    AsyncOperation _asyncOperation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void GameOver()
    {
        _asyncOperation = SceneManager.LoadSceneAsync(0);
        _asyncOperation.allowSceneActivation = false;

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

        while (Mathf.Abs(alpha - end) > 0.01)
        {
            alpha += change;

            background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
            //Debug.Log(alpha);
            yield return new WaitForSeconds(_fadeDuration * 0.01f);
        }

        yield return new WaitForSeconds(_endingDuration);

        // fades out background if just faded in
        if (change > 0)
        {
            _asyncOperation.allowSceneActivation = true;
            StartCoroutine(FadeBackground(1, 0, -0.01f));
        }
    }
}
