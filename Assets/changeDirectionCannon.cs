using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeDirectionCannon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player"&& Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Player has entered the trigger");
            transform.Rotate(0, 180, 0);
        }
    }
}
