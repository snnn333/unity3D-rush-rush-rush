using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDoorDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        if(GetComponent<InteractiveItem>() && GetComponentInChildren<TextMesh>()){
            var amount = GetComponent<InteractiveItem>().cost;
            GetComponentInChildren<TextMesh>().text = amount.ToString();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
