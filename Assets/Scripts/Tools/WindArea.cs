using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class WindArea : MonoBehaviour
    {

        public float strength;
        public Vector3 direction;
        private GameObject player;
        private bool inWindZone;

        private MovementCharacterController controller;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Player enters wind area");
                player = other.gameObject;
                inWindZone = true;
                controller = player.GetComponent<MovementCharacterController>();

                // Lift up the player
                controller.Lift(strength, direction);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Exit the wind zone, stop lifting
            inWindZone = false;
            controller.IsLifting = false;
        }

        private void FixedUpdate()
        {
            if (inWindZone)
            {
                Debug.Log("Apply force to the player in the wind zeon");
                // // Lift up the player
                controller.Lift(strength, direction);
            }
        }

    }
}
