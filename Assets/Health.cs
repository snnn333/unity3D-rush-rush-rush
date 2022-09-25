using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update Health
    public void TakeDamage(int health)
    {
        StatisticManager.addHealthReduction(health);
        
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
        }
    }

    // Gain Health
    public void GainHealth(int health)
    {
        StatisticManager.addHealthGained(health);
        
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
