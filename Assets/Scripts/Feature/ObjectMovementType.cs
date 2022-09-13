using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class ObjectMovementType : MonoBehaviour
    {
        [Tooltip("Allows only to move the object in the directions of the faces that are pressed.")]
        public bool FixedMovement;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (other.GetComponent<MovementCharacterController>())
            {
                other.GetComponent<MovementCharacterController>().PushInFixedDirections = FixedMovement;
            }
        }
    }
}