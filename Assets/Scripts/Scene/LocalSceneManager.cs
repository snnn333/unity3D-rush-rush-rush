using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalSceneManager : MonoBehaviour
{
    public Bag bag;

    private LocalSceneManager instance;
    
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
    }

    public void EnterLevel1()
    {
        if(bag != null){
            bag.ResetBag();
        }
        SceneManager.LoadScene("Level 1");
    }
    
    public void EnterLevelSelection()
    {
        // StatisticManager.QuitAndSaveData();
        if(bag != null){
            bag.ResetBag();
        }
        SceneManager.LoadScene("Level Selector");
    }

    public void EnterMainScene()
    {
        
        // StatisticManager.QuitAndSaveData();
        if(bag != null){
            bag.ResetBag();
        }
        SceneManager.LoadScene("Start");
    }

    public void EnterControlTutorial()
    {
        SceneManager.LoadScene("Control");
    }
}
