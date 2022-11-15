using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject gamePassObj;
    
    private void OnTriggerEnter(Collider other)
    {
        // Disable player's movement
        GameObject player = GameObject.FindWithTag("Player");
        (player.GetComponent("MovementCharacterController") as MonoBehaviour).enabled = false;

        // Display the win screen
        gamePassObj.SetActive(true);

        // Disable exit and bag button
        GameObject exitButton = GameObject.FindWithTag("ExitButton");
        exitButton.SetActive(false);

        GameObject bagButton = GameObject.FindWithTag("BagButton");
        bagButton.SetActive(false);
    }
}
