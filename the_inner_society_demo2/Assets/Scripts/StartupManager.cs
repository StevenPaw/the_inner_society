using System;
using System.Collections;
using System.Collections.Generic;
using farmingsim;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    [SerializeField] private string firstSceneToLoad;
    [SerializeField] private float loadDelay;
    [SerializeField] private string farmScene;
    [SerializeField] private string settlementScene;
    [SerializeField] private string natureScene;
    [SerializeField] private string cityScene;

    private void Start()
    {
        string lastScene = PlayerPrefs.GetString("lastVisitedScene", "notused");
        if (lastScene != "notused")
        {
            firstSceneToLoad = lastScene;
        }
        
        

        StartCoroutine(LoadFirstScene());
    }
    
    IEnumerator LoadFirstScene()  //  <-  its a standalone method
    {
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(firstSceneToLoad);
    }
}
