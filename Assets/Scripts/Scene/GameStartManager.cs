using System;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    private bool hasStarted = false;
    private void Start()
    {
        if (hasStarted == false) {
            // Delete all pref data
            PlayerPrefs.DeleteAll();
            hasStarted = true;
        }
        
    }
}
