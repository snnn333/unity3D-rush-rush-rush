using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
                player = other.gameObject;
                inWindZone = true;
                controller = player.GetComponent<MovementCharacterController>();

                // Lift up the player
                controller.IsLifting = true; // Enable lift
                controller.Lift(strength, direction);

                // Stop the player from lifting after a few seconds
                // StartCoroutine(WaitThenStop());
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
            // Out of the windzone when the player flies to high away from the fan
            if (inWindZone == true && Vector3.Distance(transform.position, player.transform.position) > 15f) 
            {
                float distance =  Vector3.Distance(transform.position, player.transform.position);
                string debugMessage = String.Format("Player is too high, out of wind zone. Distance: {0}", distance);
                Debug.Log(debugMessage);
                inWindZone = false;
                controller.IsLifting = false;
            }

            if (inWindZone)
            {
                // Lift up the player
                controller.Lift(strength, direction);
            }
        }

        private IEnumerator WaitThenStop()
        {
            yield return new WaitForSeconds(1);
            inWindZone = false;

        }

    }
}
