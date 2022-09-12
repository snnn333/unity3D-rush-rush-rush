using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Bag myBag;

    private void OnTriggerEnter(Collider other)
    {
        // If we don't have this item
        // We need to iterate to find empty position
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
                        break;
                    }
                }
            }
        }
        else
        {
            thisItem.num++;
        }
    }
}
