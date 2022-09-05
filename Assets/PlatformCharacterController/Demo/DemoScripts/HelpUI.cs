using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformCharacterController
{
    public class HelpUI : MonoBehaviour
    {
        public MovementCharacterController Player;

        public Text JetPackFuel;

        public LoadScene LoadSceneScript;

        private void FixedUpdate()
        {
            if (Player)
            {
                JetPackFuel.text = ((int) Player.JetPackFuel).ToString();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadSceneScript.LoadNewScene("MainScene");
            }
        }
    }
}