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

    public GameObject usedEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (bag.ContainsItem(coin) && coin.num >= cost) {
                bag.RemoveMultipleItem(coin, cost); // Decrease the money
                UseItem();
            } else {
                DisplayMessage("Not enough moeny");
            }
        }
    }

    private void UseItem() {
        // TODO: Only support bread
        GameObject.Find("Player").GetComponent<Health>().GainHealth(1);
        Debug.Log("Gain 1 health");

        if (usedEffect) {
            GameObject spawnedVFX = Instantiate(usedEffect, transform.position, transform.rotation) as GameObject; 
            Destroy(usedEffect, 5f);
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
