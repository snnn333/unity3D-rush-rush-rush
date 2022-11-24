
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
    public AudioClip successSound;
    private Renderer myRenderer;

    public void Start()
    {
        myRenderer = GetComponent<Renderer>();
        // copy reference to the original material
        enabledMaterial = myRenderer.material;
        // Disable material
        myRenderer.material = disabledMaterial;
    }

    public void Awake()
    {
        isActived = false;
    }

    private void SpawnEffect()
    {
        GameObject spawnedVFX = Instantiate(myVFX, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
        Destroy(spawnedVFX, 5f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //  When the checkpoint is not activated
        if (other.CompareTag("Player") && !isActived)
        {
            // Play the sound
            if (successSound)
            {
                AudioSource.PlayClipAtPoint(successSound, 0.9f*Camera.main.transform.position + 0.1f*transform.position ,10f);
            }
            other.GetComponent<MovementCharacterController>().checkPointObj = gameObject.transform.parent.gameObject;
            isActived = true;
            // Enable material
            myRenderer.material = enabledMaterial;
        }

        if (other.CompareTag("Player")) {
            // VFX
            SpawnEffect();
        }
    }
}
