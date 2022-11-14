using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DestroyParent(){
        Destroy(transform.parent.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
