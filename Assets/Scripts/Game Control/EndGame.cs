/*****************************************************************************
// File Name :         EndGame.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     02/24/24
//
// Brief Description : Loads in the end game info
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {     
        //LoadEndScene();
    }

    /// <summary>
    /// Loads end scene
    /// </summary>
    public static void LoadEndScene()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);   
    }
}
