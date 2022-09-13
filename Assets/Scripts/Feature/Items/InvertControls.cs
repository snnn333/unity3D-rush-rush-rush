using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class InvertControls : MonoBehaviour
    {
        [Tooltip("This is the invert control time.")]
        public float InvertControlTime = 3;

        public GameObject Effect;

        private bool _active = true;

        private void InvertPlayerMovement(MovementCharacterController player)
        {
            _active = false;
            if (Effect)
            {
                Instantiate(Effect, player.transform.position, player.transform.rotation, player.transform);
            }

            player.InvertPlayerControls(InvertControlTime);

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _active)
            {
                InvertPlayerMovement(other.GetComponent<MovementCharacterController>());
            }
        }
    }
}