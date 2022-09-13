using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class Sprint : MonoBehaviour
    {
        [Tooltip("The speed value to add at the player speed.")]
        public float SpeedPlus = 5;

        [Tooltip("This is the time that the speed plus is active in the player.")]
        public float SprintTime = 4;

        public GameObject Effect;

        private bool _active = true;


        private void SprintPlayer(MovementCharacterController player)
        {
            _active = false;
            if (Effect)
            {
                Instantiate(Effect, transform.position, transform.rotation);
            }

            player.ChangeSpeedInTime(SpeedPlus, SprintTime);

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _active)
            {
                SprintPlayer(other.GetComponent<MovementCharacterController>());
            }
        }
    }
}