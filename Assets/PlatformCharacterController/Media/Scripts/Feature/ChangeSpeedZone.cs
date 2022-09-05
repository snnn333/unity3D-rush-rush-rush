using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class ChangeSpeedZone : MonoBehaviour
    {
        [Tooltip("This is the speed for the player in this zone")]
        public float ZoneSpeed;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<MovementCharacterController>().ChangeSpeed(ZoneSpeed);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<MovementCharacterController>().ResetOriginalSpeed();
            }
        }
    }
}