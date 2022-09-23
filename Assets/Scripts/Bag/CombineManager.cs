using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CombineManager : MonoBehaviour
{
    private static CombineManager instance;
    public Bag myBag;
    public Item keyItem;

    public Text text;

    public Item item0;
    public Item item1;
    public Item result;

    public Image item0Image;
    public Image item1Image;
    public Image resultImage;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
    }

    public static void SetItem0(Item item)
    {
        instance.item0 = item;
        if (item == null)
        {
            instance.item0Image.color = new Color(255, 255, 255, 0);
        }
        else
        {
            instance.item0Image.sprite = item.image;
            instance.item0Image.color = new Color(255, 255, 255, 255);
        }
    }
    
    public static void SetItem1(Item item)
    {
        instance.item1 = item;
        if (item == null)
        {
            instance.item1Image.color = new Color(255, 255, 255, 0);
        }
        else
        {
            instance.item1Image.sprite = item.image;
            instance.item1Image.color = new Color(255, 255, 255, 255);
        }
    }
    
    public static void SetResult(Item item)
    {
        instance.result = item;
        if (item == null)
        {
            instance.resultImage.color = new Color(255, 255, 255, 0);
        }
        else
        {
            instance.resultImage.sprite = item.image;
            instance.resultImage.color = new Color(255, 255, 255, 255);
        }
    }

    public static void Combine()
    {
        if (instance.item0 == null || instance.item1 == null || instance.item0 == instance.item1)
        {
            SetItem0(null);
            SetItem1(null);
            SetResult(null);
            instance.text.text = "This doesn't seem to be synthetic!";
            
            return;
        }

        if (instance.item0.name == "RustedKey" && instance.item1.name == "Oil" || 
            instance.item1.name == "RustedKey" && instance.item0.name == "Oil")
        {
            // Can combine
            instance.keyItem.num++;
            
            if (!instance.myBag.itemList.Contains(instance.keyItem))
            {
                for (int i = 0; i < instance.myBag.itemList.Count; i++)
                {
                    if (instance.myBag.itemList[i] == null || instance.myBag.itemList[i].num == 0)
                    {
                        instance.myBag.itemList[i] = instance.keyItem;
                        break;
                    }
                }
            }
            
            // for (int i = 0; i < instance.myBag.itemList.Count; i++)
            // {
            //     // Find empty grid
            //     if (instance.myBag.itemList[i] != null 
            //         && (instance.myBag.itemList[i].name == "RustedKey" || instance.myBag.itemList[i].name == "Oil"))
            //     {
            //         instance.myBag.itemList[i].num--;
            //     }
            // }
            instance.item0.num--;
            instance.item1.num--;

            SetItem0(null);
            SetItem1(null);
            SetResult(instance.keyItem);
            BagManager.RefreshItem();
        }
    }
    
}
