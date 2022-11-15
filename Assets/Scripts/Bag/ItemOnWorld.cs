using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Bag myBag;
    public bool hasMessage = true;
    public string itemMessage = "You picked up ";

    private void OnTriggerEnter(Collider other)
    {
        // If we don't have this item
        // We need to iterate to find empty position
        if(other.gameObject.CompareTag("Player")){

            // AddPopup(other);
            // if(Input.GetKeyDown(KeyCode.E)){
            //     AddItem(other);
            // }
            AddItem(other);
            
            // StatisticManager.AddItem(thisItem.name, Time.time);
            
            // // consume bread
            // if(thisItem.name == "Bread"){
            //     Health health = other.gameObject.GetComponent<Health>();
            //     if(health.maxHealth > health.currentHealth){
            //         health.currentHealth += 1;
            //         Destroy(this.gameObject);
            //         return;
            //     }
            // }else{
            //     // add item
            //     DisplayMessage(itemMessage + thisItem.name);
            //     myBag.AddItem(thisItem);
            //     Destroy(this.gameObject);
            // }
        }   
    }

    private void AddPopup(Collider other){
        var popupManager = other.GetComponent<TextPopUp>();
        string text = "Press 'E' to pick up "+ thisItem.name;
        if(popupManager != null){
            if(popupManager.hasPopup() && popupManager.getText() == text){
                popupManager.updatePopup();
            }else{
                popupManager.createPopup(text,other.transform.position,other.transform.rotation);
            }
        }
    }


    private void AddItem(Collider other){
        StatisticManager.AddItem(thisItem.name, Time.time);
        // consume bread
        if(thisItem.name == "Bread"){
            Health health = other.gameObject.GetComponent<Health>();
            if(health.maxHealth > health.currentHealth){
                health.currentHealth += 1;
                Destroy(this.gameObject);
                return;
            }
        }else{
            // add item
            if (hasMessage) {
                DisplayMessage(itemMessage + thisItem.name);
            }
            
            myBag.AddItem(thisItem);
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
