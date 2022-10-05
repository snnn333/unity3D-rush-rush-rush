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
                if(myBag.itemList.Contains(key) && key.num > 0){
                    DisplayMessage(successMessage);
                    Destroy(this.gameObject);
                    if(key.num <= 1){
                        key.num = 1;
                        for (int i = 0; i < myBag.itemList.Count; i++){
                            if(myBag.itemList[i] == key){
                                myBag.itemList[i] = null;
                                break;
                            }
                        }
                
                        // myBag.itemList.Remove(key);
                    }else{
                        key.num -= 1;
                    }
                }else{
                    Debug.Log(failMessage);
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
