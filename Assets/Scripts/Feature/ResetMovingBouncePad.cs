﻿// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;


// namespace PlatformCharacterController
// {
//     public class ResetMovingBouncePad : MonoBehaviour
//     {
//         [Tooltip("Time to start teleporting the player.")]
//         public float StartTeleport;

//         [Tooltip("The player wait this time to can control the player again.")]
//         public float TimeToControlPlayer;

//         [Tooltip("Effect to start teleport.")] public GameObject TeleportEffect;

//         public Transform TeleportPosition;

//         private IEnumerator TeleportPlayer(Transform player)
//         {
//             if (TeleportEffect)
//             {
//                 Instantiate(TeleportEffect, player.position + Vector3.up, player.rotation);
//             }
//             yield return new WaitForSeconds(StartTeleport);
            
            
//             player.position = TeleportPosition.position;
//         }

//         private void OnTriggerEnter(Collider other)
//         {
//             if (other.CompareTag("MovingBouncePad"))
//             {   

//                 // Minus one live after the player hitting the lava or the enemy
//                 Health health = GameObject.Find("Player").GetComponent<Health>();
//                 health.TakeDamage("Teleport", 1);
                
//                 TeleportPosition =
//                     other.GetComponent<MovementCharacterController>().checkPointObj.transform;

//                 StartCoroutine(other.GetComponent<MovementCharacterController>()
//                     .DeactivatePlayerControlByTime(TimeToControlPlayer));
//                 StartCoroutine(TeleportPlayer(other.transform));

//                 if (health.currentHealth > 0) {
//                     StartCoroutine(ShowDeathUI());
//                 }
//             }
//         }
//     }
// }