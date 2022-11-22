using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


namespace PlatformCharacterController
{
    public class TeleportToNextScene : MonoBehaviour
    {
        public bool IsNeedKeyPress;
        private bool _IsEntered = false;
        
        [Tooltip("Name of the next scene.")]
        public string scenename;
        public Animator transaction;

        public GameObject LevelTitle;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && IsNeedKeyPress == false)
            {
                Debug.Log("sceneName to load: " + scenename);
                SceneManager.LoadScene(scenename);
            }
            else if (other.CompareTag("Player") && IsNeedKeyPress == true)
            {
                _IsEntered = true;
                DisplayMessage("Press [E] to Enter Level");
                Debug.Log("Needs key press to enter the level");
            }
        }

        private void Update()
        {
            if (_IsEntered && Input.GetKeyDown(KeyCode.E))
            {
                // SceneManager.LoadScene(scenename);
                LoadNextLevel();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _IsEntered = false;
        }

        private void DisplayMessage(string msg){
            var message = GameObject.FindGameObjectsWithTag("MessageCenter");
            if(message != null && message.Length > 0){
                message[0].GetComponent<Notification>().setMessage(msg);
            }
        }

        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevel(scenename));
        }

        IEnumerator LoadLevel(string scenename)
        {
            // Set the text
            var text = LevelTitle.GetComponent<TextMeshProUGUI>();
            text.text = scenename;
            // Play animation
            transaction.SetTrigger("Start");

            // Wait for seconds
            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene(scenename);
        }
        
    }
}