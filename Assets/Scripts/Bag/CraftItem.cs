using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftItem : MonoBehaviour
{
    public Image image;
    
    public void OnClick()
    {
        image.color = new Color(255, 255, 255, 0);
        
        if (gameObject.name == "element0")
        {
            CombineManager.SetItem0(null);
        }
        else if (gameObject.name == "element1")
        {
            CombineManager.SetItem1(null);
        }
        else if (gameObject.name == "result")
        {
            CombineManager.SetResult(null);
        }
    }
}
