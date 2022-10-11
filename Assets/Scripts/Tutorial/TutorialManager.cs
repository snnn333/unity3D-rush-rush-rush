using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUpWindows;

    // step 3
    public Bag bag;
    public Item Key;

    // step 4
    public GameObject BagButton;

    // step 6
    public Item Torch;

    // step 7
    public GameObject Ice;

    //step 8
    public float waitTime = 4f;

    private int popUpIndex = 0;
    private bool waiting = false;
    // Start is called before the first frame update
    void Start()
    {
        // if(BagButton == null){

        //     BagButton = GameObject.FindWithTag("BagButton");
        // }
        // if(Ice == null){

        //     Ice = GameObject.FindWithTag("TutorialIce");
        // }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i <popUpWindows.Length; i++){
            if(i == popUpIndex){
                Debug.Log(i);
                popUpWindows[popUpIndex].SetActive(true);
                // foreach (Transform child in popUpWindows[popUpIndex].transform)
                // {
                //     child.gameObject.SetActive(true);
                // }
                for (int a = 0; a < popUpWindows[popUpIndex].transform.childCount; a++)
                {
                    popUpWindows[popUpIndex].transform.GetChild(a).gameObject.SetActive(true);
                }
            } else {
                popUpWindows[i].SetActive(false);
            }
        }

        if(popUpIndex == 0){

            if(Input.GetKeyDown(KeyCode.Space)){
                if(!waiting){
                    StartCoroutine(waiter());
                }
            }
        } else if(popUpIndex == 1){
                        Debug.Log("1");
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)){
                if(!waiting){
                    StartCoroutine(waiter());
                }
            }
        } else if(popUpIndex == 2){
            if(Input.GetKeyDown(KeyCode.Space)){
                if(!waiting){
                    StartCoroutine(waiter());
                }
            }
        } else if(popUpIndex == 3){
            if(bag.ContainsItem(Key)){
                if(!waiting){
                    StartCoroutine(waiter());
                }
            }
        } else if(popUpIndex == 4){
            if(BagButton.GetComponent<ButtonPressed>().buttonPressed){
                if(!waiting){
                    StartCoroutine(waiter());
                }
            }
            
        } else if(popUpIndex == 5){
            if(!bag.ContainsItem(Key)){
                if(!waiting){
                    StartCoroutine(waiter());
                }
            }
        } else if(popUpIndex == 6){
            if(bag.ContainsItem(Torch)){
                if(!waiting){
                    StartCoroutine(waiter());
                }
            }
        } else if(popUpIndex == 7){
            if(Ice == null){
                if(!waiting){
                    StartCoroutine(waiter());
                }
            }
        } else if(popUpIndex == 8){
            // if(!waiting){
            //     StartCoroutine(waiter());
            // }
        } 
        // else if(popUpIndex == 9){
            
        // }
    }


    IEnumerator waiter()
    {
        //Wait for 1 seconds
        waiting = true;
        yield return new WaitForSecondsRealtime(waitTime);
        popUpIndex++;
        waiting = false;
    }
}
