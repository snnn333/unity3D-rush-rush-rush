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
        SceneManager.LoadScene("Jungle - Side View 3");
    }
    
    public static void EnterMainScene()
    {
        StatisticManager.QuitAndSaveData();
        SceneManager.LoadScene("Start");
    }
}
