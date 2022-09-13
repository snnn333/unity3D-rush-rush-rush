using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class EnableDisableAllSkills : MonoBehaviour
    {
        [Header("Activate all skills")] [Tooltip("Enable player to use all skills if true, disable all if false")]
        public bool EnableAllSkills;

        public bool DestroyIfActive;

        public GameObject Effect;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var player = other.GetComponent<MovementCharacterController>();

            player.ActivateDeactivateJump(EnableAllSkills);
            player.ActivateDeactivateDoubleJump(EnableAllSkills);
            player.ActivateDeactivateDash(EnableAllSkills);
            player.ActivateDeactivateSlowFall(EnableAllSkills);
            player.ActivateDeactivateJetpack(EnableAllSkills);

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