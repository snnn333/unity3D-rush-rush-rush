
using System;
using System.Collections;
using System.Collections.Generic;
using PlatformCharacterController;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool isActived;

    public void Awake()
    {
        isActived = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("111");
        if (other.CompareTag("Player") && !isActived)
        {
            other.GetComponent<MovementCharacterController>().checkPointObj = gameObject.transform.parent.gameObject;
            isActived = true;
        }
    }
}
