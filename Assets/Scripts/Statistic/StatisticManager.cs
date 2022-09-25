using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StatisticManager : MonoBehaviour
{
    [Serializable]
    public class StatisticFile
    {
        public string dateTime;
        public int healthReduction;
        public int healthGained;
        public float lastLevelTime;
        public float firstLevelTime;
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

    

    public void OnApplicationQuit()
    {
        // Save Data
    }
}
