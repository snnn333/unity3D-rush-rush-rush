using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class HandleSlowFall : MonoBehaviour
    {
        [Header("Activate slow fall")] [Tooltip("Enable player to slow fall if true, disable if false")]
        public bool ActivateSlowFall;

        public bool DestroyIfActive;

        public GameObject Effect;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<MovementCharacterController>().ActivateDeactivateSlowFall(ActivateSlowFall);

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