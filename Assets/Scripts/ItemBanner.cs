using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemBanner : MonoBehaviour
{
    public GameObject banner;
    public Item item;
    public string itemName;
    public float offsetY = 1.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        var position = this.transform.position;
        position.y += offsetY;
        GameObject go = Instantiate(banner, position ,Quaternion.identity) as GameObject; 
        go.transform.parent = transform;
        var tmp = go.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        if(tmp != null){
            tmp.GetComponent<TextMeshPro>().text = item.name;   
        }
        // go.transform.position.y += offsetY;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
