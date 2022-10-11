using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class InteractiveItem : MonoBehaviour
{
    public int colliderId;
    public Bag myBag;
    public Item key;
    public bool consumable = true;
    public string failMessage = "Needs key to open door";
    public string successMessage = "You opened the door!";
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("Player entered trigger");
            if (!consumable){
                Debug.Log("Item is not consumable");
                if(myBag.itemList.Contains(key)){
                    DisplayMessage(successMessage);
                    Destroy(this.gameObject);
                }else{
                    Debug.Log(failMessage);
                    DisplayMessage(failMessage);
                }
                
            }
            else{
                Debug.Log("Item is consumable");
                var result = myBag.RemoveItem(key);
                if(result){
                    DisplayMessage(successMessage);
                    Destroy(this.gameObject);
                }else{
                    DisplayMessage(failMessage);
                }
            }
        }
    }

    private void DisplayMessage(string msg){
        var message = GameObject.FindGameObjectsWithTag("MessageCenter");
        if(message != null && message.Length > 0){
            message[0].GetComponent<Notification>().setMessage(msg);
        }
        
    }
}
