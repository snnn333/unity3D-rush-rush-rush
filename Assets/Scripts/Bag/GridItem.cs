using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridItem : MonoBehaviour
{
    public Item item;
    public Image image;
    public Text numText;
    public string itemInfo;

    public GameObject itemInSlot;

    public void onClicked()
    {
        BagManager.UpdateItemInfo(itemInfo);
    }

    public void SetupGridItem(Item myItem)
    {
        if (myItem == null || myItem.num == 0)
        {
            itemInSlot.SetActive(false);
            return;
        }

        item = myItem;
        image.sprite = myItem.image;
        numText.text = myItem.num.ToString();
        itemInfo = myItem.description;
    }
}
