using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject gameOverObj;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update Health
    public void TakeDamage(string sourceName, int health)
    {
        StatisticManager.AddHealthReduction(sourceName, health);
        
        currentHealth -= health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            
            // Die
            Debug.Log("Player Died");
            gameOverObj.SetActive(true);
            StatisticManager.QuitAndSaveData();
            // Application.Quit();
        }
    }

    // Gain Health
    public void GainHealth(int health)
    {
        StatisticManager.AddHealthGained(health);
        
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
