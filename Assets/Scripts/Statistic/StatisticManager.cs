using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.SceneManagement;


public class StatisticManager : MonoBehaviour
{
    [Serializable]
    public class Damage
    {
        public string damageSource;
        public int damageCount;

        public Damage(string name, int count)
        {
            damageSource = name;
            damageCount = count;
        }
        
    }
    
    [Serializable]
    public class ItemData
    {
        public string name;
        public float time;

        public ItemData(string itemName, float itemTime)
        {
            name = itemName;
            time = itemTime;
        }
        
    }
    

    [Serializable]
    public class StatisticFile
    {
        public string dateTime; // End date time, primary key to identify this game.
        public int healthReduction; // Total health reduction
        public int healthGained; // Total health gained
        public float totalTime;
        public string sceneName;

        public List<Damage> damageList;
        public List<ItemData> itemDataList;
    }
    
    private static StatisticManager instance;

    public StatisticFile statisticFile;
    

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;

        statisticFile = new StatisticFile();
        
        statisticFile.healthReduction = 0;
        statisticFile.healthGained = 0;
        statisticFile.damageList = new List<Damage>();
        statisticFile.itemDataList = new List<ItemData>();
    }

    public static void ResetData()
    {
        instance.statisticFile.damageList.Clear();
        instance.statisticFile.itemDataList.Clear();
        instance.statisticFile.healthReduction = 0;
        instance.statisticFile.healthGained = 0;
    }
    
    public static void AddHealthReduction(string sourceName, int addNum)
    {
        Damage damage = new Damage(sourceName, addNum);
        instance.statisticFile.damageList.Add(damage);
        instance.statisticFile.healthReduction += addNum;
    }
    
    public static void AddHealthGained(int addNum)
    {
        instance.statisticFile.healthGained += addNum;
    }
    
    public static void AddItem(string itemName, float itemTime)
    {
        instance.statisticFile.itemDataList.Add(new ItemData(itemName, itemTime));
    }

    private static void PostToDatabase()
    {
        Debug.Log("Post the data to the database");
        RestClient.Post("https://csci-526-rushrushrush-default-rtdb.firebaseio.com/.json ",
            instance.statisticFile);
    }

    public static void QuitAndSaveData()
    {
        // Save Data
        //
        if (SceneManager.GetActiveScene().name == "Start")
        {
            return;
        }
        instance.statisticFile.totalTime = Time.time;
        instance.statisticFile.dateTime = DateTime.Now.ToString();
        instance.statisticFile.sceneName = SceneManager.GetActiveScene().name;
        PostToDatabase();
        Debug.Log(instance.statisticFile.totalTime);
        // TODO: Return to main panel

        ResetData();
    }
}
