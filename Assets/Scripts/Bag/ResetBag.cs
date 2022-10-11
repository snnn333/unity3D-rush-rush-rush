using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBag : MonoBehaviour
{
    public Bag myBag;
    // Start is called before the first frame update
    void Start()
    {
        if(myBag != null){
            myBag.ResetBag();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        if(myBag != null){
            myBag.ResetBag();
        }
    }
}
