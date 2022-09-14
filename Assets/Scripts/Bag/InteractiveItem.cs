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
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            if(myBag.itemList.Contains(key)){
                Destroy(this.gameObject);
                Debug.Log("Door Opened");
            }else{
                Debug.Log("Needs key to open door");
            }
        }
        
    }
}