using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalSceneManager : MonoBehaviour
{
    private static LocalSceneManager instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
    }

    public static void EnterLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }
    
    public static void EnterLevelSelection()
    {
        SceneManager.LoadScene("Level Selector");
    }

    public static void EnterMainScene()
    {
        StatisticManager.QuitAndSaveData();
        SceneManager.LoadScene("Start");
    }
}
