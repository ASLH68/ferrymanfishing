using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishUIController : MonoBehaviour
{
    public static FishUIController Instance;
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
    /// it calls in the animator to animate the UI of the fish caught, has old code if necessary
    /// </summary>
    /// <returns></returns>
    public IEnumerator CaughtFishUI(GameObject targetPosition, GameObject _fishImageObject)
    {
        _uiAnimator.Play("FadeIn");
        _Lsplash.Play();
        _Rsplash.Play();
        yield return new WaitForSeconds(_fadeDuration);
        _Lsplash.Stop();
        _Rsplash.Stop();
        _uiAnimator.Play("FadeOut");
        targetPosition.SetActive(true);
        //targetPosition.GetComponent<UnityEngine.UI.Image>().sprite = _fishImageObject.GetComponent<UnityEngine.UI.Image>().sprite;
        //// fade from opaque to transparent
        //if (fadeAway)
        //{
        //    uIDisplay.gameObject.SetActive(true);
        //    // loop over 1 second backwards
        //    for (float i = 1; i >= 0; i -= Time.deltaTime)
        //    {
        //        // set color with i as alpha
        //        uIDisplay.color = new Color(1, 1, 1, i);
        //        yield return new WaitForSeconds(_fadeDuration);
        //    }
        //}
        //// fade from transparent to opaque
        //else
        //{
        //    // loop over 1 second
        //    for (float i = 0; i <= 1; i += Time.deltaTime)
        //    {
        //        // set color with i as alpha
        //        uIDisplay.color = new Color(1, 1, 1, i);
        //        yield return new WaitForSeconds(_fadeDuration);
        //    }
        //    uIDisplay.gameObject.SetActive(false);
        //}
    }
}
