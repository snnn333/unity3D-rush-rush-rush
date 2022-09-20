using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class BagManger : MonoBehaviour
{
    private static BagManger instance;

    public Bag myBag;
    public GameObject grid;
    //public GridItem gridItemPrefab;
    public GameObject emptySlot;
    public Text itemInfo;

    public List<GameObject> slots = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
    }

    private void OnEnable()
    {
        RefreshItem();
        instance.itemInfo.text = "This is your bag!";
    }

    public static void UpdateItemInfo(string info)
    {
        instance.itemInfo.text = info;
    }

    // Create an item on bag
    // public static void CreateNewItem(Item item)
    // {
    //     // create UI obj
    //     GridItem newItem = Instantiate(instance.gridItemPrefab, instance.grid.transform.position, Quaternion.identity);
    //     newItem.gameObject.transform.SetParent(instance.grid.transform);
    //
    //     // set value
    //     newItem.item = item;
    //     newItem.image.sprite = item.image;
    //     newItem.num.text = item.num.ToString();
    // }

    // After logical calculation
    // Call this function to update UI on bag
    public static void RefreshItem()
    {
        // Destroy all UI bag item
        for (int i = 0; i < instance.grid.transform.childCount; i++)
        {
            Destroy(instance.grid.transform.GetChild(i).gameObject);
        }
        
        instance.slots.Clear();
        
        
        // Recreate all UI bag item
        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            //CreateNewItem(instance.myBag.itemList[i]);
            instance.slots.Add((Instantiate(instance.emptySlot)));
            instance.slots[i].transform.SetParent(instance.grid.transform);
            
            instance.slots[i].GetComponent<GridItem>().SetupGridItem(instance.myBag.itemList[i]);
        }
    }
}
