using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class Fuel : MonoBehaviour
    {
        [Tooltip("The amount of fuel to set to the player.")]
        public float FuelValue;

        public GameObject Effect;

        private bool _active = true;

        private void AddFuel(MovementCharacterController player)
        {
            _active = false;
            if (Effect)
            {
                Instantiate(Effect, transform.position, transform.rotation);
            }

            player.AddFuel(FuelValue);

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _active)
            {
                AddFuel(other.GetComponent<MovementCharacterController>());
            }
        }
    }
}