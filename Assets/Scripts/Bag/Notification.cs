using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Notification : MonoBehaviour
{

    public string message = "welcome";
    public bool displayStr = false;
    public GameObject notificationObject = null;
    public float disappearTime = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        // displayStr = false;
        var noteList = GameObject.FindGameObjectsWithTag("Notification");
        if(noteList != null && noteList.Length > 0){
            notificationObject =  noteList[0];
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(notificationObject != null){
            if(displayStr){
                // StartCoroutine(ShowMessage());


            }else{
                notificationObject.SetActive(false);
            }
        }
        
        
    }
    
    // set and display message
    public void setMessage(string msg){
        message = msg;
        displayStr = true;
        StartCoroutine(ShowMessage());
    }

    private IEnumerator ShowMessage(){
        notificationObject.SetActive (true);
        notificationObject.transform.GetChild(0).gameObject.SetActive(true);
        notificationObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = message;

        yield return new WaitForSeconds(disappearTime);

        notificationObject.SetActive (false);
        displayStr = false;
    }
}
