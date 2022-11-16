
using System;
using System.Collections;
using System.Collections.Generic;
using PlatformCharacterController;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool isActived;
    // Change the material when the player 
    public Material disabledMaterial;
    private Material enabledMaterial;
    // VFX
    public GameObject myVFX;
    private Renderer renderer;

    public void Start()
    {
        renderer = GetComponent<Renderer>();
        // copy reference to the original material
        enabledMaterial = renderer.material;
        // Disable material
        renderer.material = disabledMaterial;
    }

    public void Awake()
    {
        isActived = false;
    }

    private void SpawnEffect()
    {
        GameObject spawnedVFX = Instantiate(myVFX, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject; Destroy(spawnedVFX, 5f);
        Destroy(spawnedVFX, 5f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("111");
        if (other.CompareTag("Player") && !isActived)
        {
            other.GetComponent<MovementCharacterController>().checkPointObj = gameObject.transform.parent.gameObject;
            isActived = true;
            // Enable material
            renderer.material = enabledMaterial;
            // VFX
            SpawnEffect();
        }
    }
}
