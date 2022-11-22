using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PlatformCharacterController
{
    public class ResetMovingBouncePad : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {   

                // Reset the moving bounce pad
                GameObject.FindWithTag("MovingBouncePad").GetComponent<MoveTransportPlatform>().Reset();
            }
        }
    }
}