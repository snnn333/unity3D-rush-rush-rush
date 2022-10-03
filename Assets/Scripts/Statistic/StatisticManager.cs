using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;


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
    public class StatisticFile
    {
        public string dateTime; // End date time, primary key to identify this game.
        public int healthReduction; // Total health reduction
        public int healthGained; // Total health gained
        public float lastLevelTime; // Time of last level started
        public float firstLevelTime; // Time span of first level
        public float totalTime;

        public List<Damage> damageList;
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
        statisticFile.lastLevelTime = Time.time;
        statisticFile.firstLevelTime = Time.time;
        statisticFile.damageList = new List<Damage>();
    }

    public static void addHealthReduction(string sourceName, int addNum)
    {
        Damage damage = new Damage(sourceName, addNum);
        instance.statisticFile.damageList.Add(damage);
        instance.statisticFile.healthReduction += addNum;
    }
    
    public static void addHealthGained(int addNum)
    {
        instance.statisticFile.healthGained += addNum;
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
        instance.statisticFile.totalTime = Time.time;
        instance.statisticFile.dateTime = DateTime.Now.ToString();
        PostToDatabase();
        Debug.Log(instance.statisticFile.totalTime);
        // TODO: Return to main panel
    }
}
