﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
                //Instantiate(TeleportEffect, player.position, player.rotation);
            }

            player.position = TeleportPosition.position;
        }

        private IEnumerator ShowDeathUI() {
            // Enable death UI for a short time
            Image deathUI = GameObject.FindWithTag("DeathUI").GetComponent<Image>();
            deathUI.enabled = true;
            yield return new WaitForSeconds(3);
            deathUI.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {   

                // Minus one live after the player hitting the lava or the enemy
                GameObject.Find("Player").GetComponent<Health>().TakeDamage("Teleport", 1);
                
                TeleportPosition =
                    other.GetComponent<MovementCharacterController>().checkPointObj.transform;

                StartCoroutine(other.GetComponent<MovementCharacterController>()
                    .DeactivatePlayerControlByTime(TimeToControlPlayer));
                StartCoroutine(TeleportPlayer(other.transform));

                StartCoroutine(ShowDeathUI());
            }
        }
    }
}