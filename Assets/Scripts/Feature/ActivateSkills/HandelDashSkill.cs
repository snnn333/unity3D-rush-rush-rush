using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class HandelDashSkill : MonoBehaviour
    {
        [Header("Activate dash")] [Tooltip("Enable player to dash if true, disable if false")]
        public bool ActivateDash;

        public bool DestroyIfActive;

        public GameObject Effect;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<MovementCharacterController>().ActivateDeactivateDash(ActivateDash);

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