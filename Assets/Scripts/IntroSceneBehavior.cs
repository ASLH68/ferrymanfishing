using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IntroSceneBehavior : MonoBehaviour
{
    public static IntroSceneBehavior Instance;
    [SerializeField]private GameObject _castScreen;

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

    // Start is called before the first frame update
    void Start()
    {
        _castScreen.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastingScreen(bool val)
    {
        _castScreen.gameObject.SetActive(val);
    }
}
