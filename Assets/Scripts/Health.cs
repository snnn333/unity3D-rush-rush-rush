using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject gameOverObj;

    public AudioSource audioSource;
    public AudioClip deadClip;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        audioSource = gameObject.GetComponent<AudioSource>();
        deadClip = Resources.Load<AudioClip>("Audio/dead");
        audioSource.clip = deadClip;
    }

    // Update Health
    public void TakeDamage(string sourceName, int health)
    {
        StatisticManager.AddHealthReduction(sourceName, health);
        
        
        currentHealth -= health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            //audioSource.clip = deadClip;
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            
            // Die
            Debug.Log("Player Died");
            gameOverObj.SetActive(true);
            StatisticManager.QuitAndSaveData();

            // Disable player's movement
            GameObject player = GameObject.FindWithTag("Player");
            (player.GetComponent("MovementCharacterController") as MonoBehaviour).enabled = false;
            // Application.Quit();
        }
        else
        {
            audioSource.Play();
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
