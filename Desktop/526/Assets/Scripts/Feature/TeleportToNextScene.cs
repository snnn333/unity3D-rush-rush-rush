using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformCharacterController
{
    public class TeleportToNextScene : MonoBehaviour
    {
        [Tooltip("Name of the next scene.")]
        public string scenename;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("sceneName to load: " + scenename);
                SceneManager.LoadScene(scenename);
            }
        }
    }
}