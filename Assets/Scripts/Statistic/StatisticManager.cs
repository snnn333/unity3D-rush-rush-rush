using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;


public class StatisticManager : MonoBehaviour
{
    [Serializable]
    public class StatisticFile
    {
        public string dateTime; // End date time, primary key to identify this game.
        public int healthReduction; // Total health reduction
        public int healthGained; // Total health gained
        public float lastLevelTime; // Time of last level started
        public float firstLevelTime; // Time span of first level
        public float totalTime;
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
    }

    public static void addHealthReduction(int addNum)
    {
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
