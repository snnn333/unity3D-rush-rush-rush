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
        BagManger.UpdateItemInfo(itemInfo);
    }

    public void SetupGridItem(Item item)
    {
        if (item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }

        image.sprite = item.image;
        numText.text = item.num.ToString();
        itemInfo = item.description;
    }
}
