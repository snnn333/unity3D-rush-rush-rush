using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GameItem : MonoBehaviour
{
    public InventoryObject inventory;
    public int colliderId;
    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            Item _item = new Item(player.inventory.database.GetItem[colliderId]);
            Debug.Log(colliderId);
            if(player.inventory.ContainsItemId(colliderId)){
                player.inventory.RemoveItemId(_item);
                Object.Destroy(this.gameObject);
                Debug.Log("Door unlocked!");
            }else{
                Debug.Log("you need key");
                // this.gameObject.GetComponentInChildren<TextMesh>().text = "You need key!";
                // this.gameObject.GetComponent<Text>().GetComponent<TextMesh>().text = "You need key!";
            }
            
        }
    }
}