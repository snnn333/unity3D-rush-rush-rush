using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class JumpingPlatform : MonoBehaviour
    {
        [Tooltip("This is the jumping forze of this plataform")]
        public float JumpForze = 4;

        public Animator PlatformAnimator;

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            //make the player jump
            other.GetComponent<MovementCharacterController>().Jump(JumpForze);
            //animate platform if exist animator
            if (PlatformAnimator)
            {
                PlatformAnimator.SetTrigger("In");
            }
        }
    }
}