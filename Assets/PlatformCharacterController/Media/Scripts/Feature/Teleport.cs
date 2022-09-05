using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class Teleport : MonoBehaviour
    {
        [Tooltip("Time to start teleporting the player.")]
        public float StartTeleport;

        [Tooltip("The player wait this time to can control the player again.")]
        public float TimeToControlPlayer;

        [Tooltip("Effect to start teleport.")] public GameObject TeleportEffect;

        public Transform TeleportPosition;

        private IEnumerator TeleportPlayer(Transform player)
        {
            yield return new WaitForSeconds(StartTeleport);
            if (TeleportEffect)
            {
                Instantiate(TeleportEffect, player.position, player.rotation);
            }

            player.position = TeleportPosition.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(other.GetComponent<MovementCharacterController>()
                    .DeactivatePlayerControlByTime(TimeToControlPlayer));
                StartCoroutine(TeleportPlayer(other.transform));
            }
        }
    }
}