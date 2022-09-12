using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class HandleDoubleJumpSkill : MonoBehaviour
    {
        [Header("Activate doblejump")] [Tooltip("Enable player doblejump if true, disable if false")]
        public bool ActivateDoubleJump;

        public bool DestroyIfActive;

        public GameObject Effect;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<MovementCharacterController>().ActivateDeactivateDoubleJump(ActivateDoubleJump);

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