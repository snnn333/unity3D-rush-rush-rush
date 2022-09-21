using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class TeleportOnTrigger : MonoBehaviour
    {
        public Transform TeleportPosition;

        [Tooltip("The name of the collider")] public string ColliderName;

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == ColliderName)
            {
                other.transform.position = TeleportPosition.position;
                other.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
        }
    }
}