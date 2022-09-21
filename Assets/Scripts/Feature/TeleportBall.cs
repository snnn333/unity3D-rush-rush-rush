using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class TeleportBall : MonoBehaviour
    {
		public Transform TeleportPosition;
        public Rigidbody rb;

		void Start() {
			rb = GetComponent<Rigidbody>();
		}

        void RespawnBall()
        {
            transform.position = TeleportPosition.position;
            rb.velocity = new Vector3(0, 0, 0);
            // transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }

        private void FixedUpdate()
        {	
			// Teleport to the original position after falling under the lava
            if (transform.position.y < -2)
            {
                RespawnBall();
            }
        }
    }
}