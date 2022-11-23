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
    public int cost = 1; // number of item to deduct from item number.
    public bool consumable = true;
    public string failMessage = "Needs key to open door";
    public string successMessage = "You opened the door!";
    // VFX
    public GameObject myVFX;

    // Audio
    public AudioClip pickupSound;

    private void SpawnEffect()
    {
        if (myVFX != null) {
            GameObject spawnedVFX = Instantiate(myVFX, transform.position, transform.rotation) as GameObject; 
            Destroy(spawnedVFX, 5f);
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("Player entered trigger");

            // Play the sound
            if (pickupSound != null) {
                AudioSource.PlayClipAtPoint(pickupSound, 0.9f*Camera.main.transform.position + 0.1f*transform.position ,10f);
            }
            
            if (!consumable){
                Debug.Log("Item is not consumable");
                if(myBag.itemList.Contains(key)){
                    DisplayMessage(successMessage);
    
                    Destroy(this.gameObject);
                    SpawnEffect();
                }else{
                    Debug.Log(failMessage);
                    DisplayMessage(failMessage);
                }
                
            }
            else{
                Debug.Log("Item is consumable");
                var result = myBag.RemoveMultipleItem(key,cost);
                if(result){
                    DisplayMessage(successMessage);
                    
                    Destroy(this.gameObject);
                    SpawnEffect();
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
