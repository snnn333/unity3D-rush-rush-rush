using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Bag myBag;
    public string itemMessage = "You picked up ";

    private void OnTriggerEnter(Collider other)
    {
        // If we don't have this item
        // We need to iterate to find empty position
        if(other.gameObject.CompareTag("Player")){
            StatisticManager.AddItem(thisItem.name, Time.time);
            
            if(thisItem.name == "Bread"){
                Health health = other.gameObject.GetComponent<Health>();
                if(health.maxHealth > health.currentHealth){
                    health.currentHealth += 1;
                    Destroy(this.gameObject);
                    return;
                }
            }

            DisplayMessage(itemMessage + thisItem.name);
            if (!myBag.itemList.Contains(thisItem))
            {
                if (other.gameObject.CompareTag("Player"))
                {

                    for (int i = 0; i < myBag.itemList.Count; i++)
                    {
                        // Find empty grid
                        if (myBag.itemList[i] == null)
                        {
                            myBag.itemList[i] = thisItem;
                            myBag.itemList[i].num = 1;
                            break;
                        }
                    }
                }
            }
            else
            {
                thisItem.num++;
            }
            Destroy(this.gameObject);
        }   
        
    }

    private void DisplayMessage(string msg){
        var message = GameObject.FindGameObjectsWithTag("MessageCenter");
        if(message != null && message.Length > 0){
            message[0].GetComponent<Notification>().setMessage(msg);
        }
        
    }
}
