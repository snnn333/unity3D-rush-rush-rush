using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


namespace PlatformCharacterController
{
    public class TeleportToNextScene : MonoBehaviour
    {
        public bool IsNeedKeyPress;
        private bool _IsEntered = false;
        
        [Tooltip("Name of the next scene.")]
        public string scenename;
        public int TotalDiamond;
        public Animator transaction;

        public GameObject LevelTitle;

        public GameObject EnterEffect;
        public AudioClip EnterSound;

        public GameObject DiamondTextInfo;
        public GameObject DiamondText;

        public GameObject LevelTextInfo;

        public Bag bag;

        private void Start()
        {
            DiamondTextInfo.SetActive(false);
            LevelTextInfo.SetActive(true);
        }

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

                // Update the diamond count
                if (DiamondText) {
                    LevelTextInfo.SetActive(false);
                    DiamondTextInfo.SetActive(true);
                    int maxDiamondCount = PlayerPrefs.GetInt(scenename + "-MaxDiamondCount", 0);
                    DiamondText.GetComponent<Text>().text = maxDiamondCount.ToString() + "/" + TotalDiamond.ToString();

                }

                // Display the level title
                string scenetitle = GetLevelTitle(scenename);

                if (scenetitle == "") {
                    GameObject.Find("LevelTitleText").GetComponent<TextMeshProUGUI>().text = scenename;
                } else {
                    GameObject.Find("LevelTitleText").GetComponent<TextMeshProUGUI>().text = scenename + "-" + scenetitle;
                }
                
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
            DiamondTextInfo.SetActive(false);
            LevelTextInfo.SetActive(true);

            // Reset level title
            GameObject.Find("LevelTitleText").GetComponent<TextMeshProUGUI>().text = "Main Map";
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

        private string GetLevelTitle(string scenename) {
            if (scenename == "Level 1") {
                return "Bullet Hill";
            } else if (scenename == "Level 2") {
                return "Ruined Castle";
            } else if (scenename == "Level 3") {
                return "Windy Beach";
            } else if (scenename == "Level 4") {
                return "Icy Mountain";
            } else {
                return "";
            }
        }

        IEnumerator LoadLevel(string scenename)
        {
            // Reset the bag
            if(bag != null){
                bag.ResetBag();
            }

            // Start entering effect
            GameObject player = GameObject.FindWithTag("Player");
            
            // Start effect and sound
            if (EnterSound) {
                AudioSource.PlayClipAtPoint(EnterSound, 0.9f*Camera.main.transform.position + 0.1f*transform.position, 10f);
            }
            if (EnterEffect != null) {
                Instantiate(EnterEffect, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
           
            // Disable player's movement
            (player.GetComponent("MovementCharacterController") as MonoBehaviour).enabled = false;
            // player.GetComponent<MovementCharacterController>().Jump(2);
            yield return new WaitForSeconds(1f);

            // Play transition animation
            transaction.SetTrigger("Start");

            // Wait for seconds
            yield return new WaitForSeconds(1f);
            // Set the text
            var text = LevelTitle.GetComponent<TextMeshProUGUI>();
            text.text = scenename + "\n" + GetLevelTitle(scenename);
            
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(scenename);
        }
        
    }
}