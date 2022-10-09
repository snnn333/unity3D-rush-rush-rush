using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject gamePassObj;
    
    private void OnTriggerEnter(Collider other)
    {
        gamePassObj.SetActive(true);
    }
}
