using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{   
    public Item item;
    public string itemName;
    public int num;
    public int cost;
    public Item coin;
    public Bag bag;
    private bool _IsEntered = false;
    // Start is called before the first frame update
    void Start()
    {   
        if(item != null){
            itemName = item.name;
        }
    }

    private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _IsEntered = true;
                DisplayMessage("Press [E] to Buy "+itemName);
                Debug.Log("Needs key press to enter the level");
            }
        }

    private void OnTriggerExit(Collider other)
        {
            _IsEntered = false;
        }

    // Update is called once per frame
    void Update()
    {
        if (_IsEntered && Input.GetKeyDown(KeyCode.E))
        {
            if(bag.ContainsItem(item) && item.num >= cost){
                for(int i = 0; i < num; i++){
                    bag.AddItem(item);
                }
                bag.RemoveMultipleItem(coin,cost);

                DisplayMessage("Successfully bought " + itemName);
            }else{
                DisplayMessage("You need " + cost + " " + coin.name + " to buy "+itemName);
            }
        }

        // if(_IsEntered){
        //     DisplayMessage("Press [E] to Buy "+itemName);
        // }
    }

    private void DisplayMessage(string msg){
        var message = GameObject.FindGameObjectsWithTag("MessageCenter");
        if(message != null && message.Length > 0){
            message[0].GetComponent<Notification>().setMessage(msg);
        }
    }
}
