using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public GameObject gamePassObj;
    public int passLevel;
    public Bag bag;

    public Item diamond;
    
    
    private void OnTriggerEnter(Collider other)
    {
        // Disable player's movement
        GameObject player = GameObject.FindWithTag("Player");
        (player.GetComponent("MovementCharacterController") as MonoBehaviour).enabled = false;

        // Display the win screen
        gamePassObj.SetActive(true);

        // // empty backpack
        // if(bag != null){
        //     bag.ResetBag();
        // }

        GameManager.passLevel(passLevel);

        // Disable exit and bag button
        GameObject exitButton = GameObject.FindWithTag("ExitButton");
        exitButton.SetActive(false);

        // GameObject bagButton = GameObject.FindWithTag("BagButton");
        // bagButton.SetActive(false);
        
        // Save the diamond count
        if (diamond == null) return;
        string sceneName = SceneManager.GetActiveScene().name;
        int prevMaxDiamondCount = PlayerPrefs.GetInt(sceneName + "-MaxDiamondCount", 0);
        if (diamond.num > prevMaxDiamondCount) {
            Debug.Log("Update the max diamond count of " + sceneName + "from" + prevMaxDiamondCount + "to" + diamond.num);
            PlayerPrefs.SetInt(sceneName + "-MaxDiamondCount", diamond.num);
            PlayerPrefs.Save();
        }
    }
}
