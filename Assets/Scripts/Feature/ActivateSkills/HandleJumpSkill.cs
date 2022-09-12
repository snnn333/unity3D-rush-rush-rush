using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class HandleJumpSkill : MonoBehaviour
    {
        [Header("Activate jump")] [Tooltip("Enable player jump if true, disable if false")]
        public bool ActivateJump;

        public bool DestroyIfActive;

        public GameObject Effect;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            //make the player jump
            other.GetComponent<MovementCharacterController>().ActivateDeactivateJump(ActivateJump);

            if (Effect)
            {
                Instantiate(Effect, transform.position, transform.rotation);
            }

            if (DestroyIfActive)
            {
                Destroy(gameObject);
            }
        }
    }
}