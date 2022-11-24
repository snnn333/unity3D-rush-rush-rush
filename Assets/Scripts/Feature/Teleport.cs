using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace PlatformCharacterController
{
    public class Teleport : MonoBehaviour
    {
        [Tooltip("Time to start teleporting the player.")]
        public float StartTeleport;

        [Tooltip("The player wait this time to can control the player again.")]
        public float TimeToControlPlayer;

        [Tooltip("Effect to start teleport.")] public GameObject TeleportEffect;

        [Tooltip("Position to teleport the player.")] public Transform TeleportPosition;

        private Animator transition;
        private TextMeshProUGUI _textGUI;

        private IEnumerator TeleportPlayer(Transform player)
        {
            if (TeleportEffect)
            {
                Instantiate(TeleportEffect, player.position + Vector3.up, player.rotation);
            }
            yield return new WaitForSeconds(StartTeleport);
            
            
            player.position = TeleportPosition.position - 2 * Vector3.forward;
            player.rotation = Quaternion.identity;
        }

        private IEnumerator ShowDeathUI() {
            // yield return new WaitForSeconds(1);
            // // Enable death UI for a short time
            // Image deathUI = GameObject.FindWithTag("DeathUI").GetComponent<Image>();
            // deathUI.enabled = true;
            // yield return new WaitForSeconds(2);
            // deathUI.enabled = false;

            // Set the text
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(1);

            if (_textGUI != null) {
                _textGUI.text = "Too Bad!";
            }

            yield return new WaitForSeconds(2f);

            if (_textGUI != null) {
                _textGUI.text = "";
            }
            
            transition.SetTrigger("End");
            yield return new WaitForSeconds(1);

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {   

                // Minus one live after the player hitting the lava or the enemy
                Health health = GameObject.Find("Player").GetComponent<Health>();
                health.TakeDamage("Teleport", 1);
                
                TeleportPosition =
                    other.GetComponent<MovementCharacterController>().checkPointObj.transform;

                StartCoroutine(other.GetComponent<MovementCharacterController>()
                    .DeactivatePlayerControlByTime(TimeToControlPlayer));
                StartCoroutine(TeleportPlayer(other.transform));

                if (health.currentHealth > 0) {
                    GameObject crossFadeImage = GameObject.Find("CrossFadeImage");
                    _textGUI = GameObject.Find("LevelTitle").GetComponent<TextMeshProUGUI>();
                    if (crossFadeImage == null) return;
                    transition = crossFadeImage.GetComponent<Animator>();
                    if (transition == null) return;
                    StartCoroutine(ShowDeathUI());
                }
            }
        }
    }
}