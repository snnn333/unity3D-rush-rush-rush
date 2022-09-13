using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class HandleJetpackSkill : MonoBehaviour
    {
        [Header("Activate jetpack")] [Tooltip("Enable player to use jetpack if true, disable if false")]
        public bool ActivateJetpack;

        public bool DestroyIfActive;

        public GameObject Effect;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<MovementCharacterController>().ActivateDeactivateJetpack(ActivateJetpack);

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