using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{   
    // public Item item;
    // public string itemName;
    // public int num;
    public int cost;
    public Item coin;
    public Bag bag;

    public AudioClip useSound;

    public GameObject usedEffect;

    public AudioClip rejectSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (bag.ContainsItem(coin) && coin.num >= cost) {
                Health healthManager = GameObject.Find("Player").GetComponent<Health>();
                // Do not use the item if the player has full health
                if (healthManager.currentHealth == healthManager.maxHealth) {
                    DisplayMessage("Your health is full!");
                    if (rejectSound != null) {
                        AudioSource.PlayClipAtPoint(rejectSound, 0.9f*Camera.main.transform.position + 0.1f*transform.position ,10f);
                    }
                    return;
                }
                bag.RemoveMultipleItem(coin, cost); // Decrease the money
                UseItem();
            } else {
                DisplayMessage("Insufficient Coins!");
                if (rejectSound != null) {
                        AudioSource.PlayClipAtPoint(rejectSound, 0.9f*Camera.main.transform.position + 0.1f*transform.position ,10f);
                }
            }
        }
    }

    private void UseItem() {
        // TODO: Only support bread
        // Play use sound
        if (useSound != null) {
                AudioSource.PlayClipAtPoint(useSound, 0.9f*Camera.main.transform.position + 0.1f*transform.position ,10f);
        }

        Health healthManager = GameObject.Find("Player").GetComponent<Health>();
        
        healthManager.GainHealth(1);
        Debug.Log("Gain 1 health");

        if (usedEffect) {
            GameObject spawnedVFX = Instantiate(usedEffect, transform.position, transform.rotation) as GameObject; 
        }

        Destroy(gameObject);
    }

    private void DisplayMessage(string msg){
        var message = GameObject.FindGameObjectsWithTag("MessageCenter");
        if(message != null && message.Length > 0){
            message[0].GetComponent<Notification>().setMessage(msg);
        }
    }
}
