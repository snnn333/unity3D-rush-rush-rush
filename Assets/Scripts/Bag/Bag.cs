using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New bag", menuName = "Bag/New Bag")]
public class Bag : ScriptableObject
{
    public List<Item> itemList = new List<Item>();

    public void ResetBag(){
        for (int i = 0; i < itemList.Count; i++){
            itemList[i] = null;
        }
    }

    public bool ContainsItem(Item item){
        return itemList.Contains(item);
    }
    public void AddItem(Item item){
        if (!itemList.Contains(item))
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                // Find empty grid
                if (itemList[i] == null)
                {
                    itemList[i] = item;
                    itemList[i].num = 1;
                    break;
                }
            }
        }
        else
        {
            item.num++;
        }
    }

    public bool RemoveItem(Item item){
        if(itemList.Contains(item) && item.num > 0){
            if(item.num <= 1){
                item.num = 0;
                for (int i = 0; i < itemList.Count; i++){
                    if(itemList[i] == item){
                        itemList[i] = null;
                        break;
                    }
                }
            }else{
                item.num--;
            }
            return true;
        }else{
            return false;
        }
    }
}
