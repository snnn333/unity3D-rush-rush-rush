using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    public bool showPopup = false;
    public GameObject popup;
    public GameObject popupInstance = null;
    // public Vector3 offset;
    // private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        if (popupInstance!= null){
            Destroy(popupInstance);
        }
    }

    public void createPopup(string text,Vector3 offset,Quaternion rotation){
        showPopup=true;
        popupInstance = Instantiate(popup, offset, rotation);
        setText(text);
    }

    public void setText(string text)
    {
        if(popupInstance!=null){
            popupInstance.GetComponent<TextMesh>().text = text;
        }
    }

    public bool hasPopup(){
        return popupInstance!=null;
    }

    public string getText(){
        if(popupInstance!=null){
            return popupInstance.GetComponent<TextMesh>().text;
        }else{
            return null;
        }
    }

    public void updatePopup(){
        showPopup = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (showPopup){
            showPopup = false;
        }
        else{
            Destroy(popupInstance);
        }
    }
}
