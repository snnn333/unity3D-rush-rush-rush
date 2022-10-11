using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
 
public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
 
public bool buttonPressed = false;
 
public void OnPointerDown(PointerEventData eventData){
     buttonPressed = true;
}
 
public void OnPointerUp(PointerEventData eventData){
    buttonPressed = false;
}
}