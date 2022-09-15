using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class TeleportBall : MonoBehaviour
    {
        Vector3 m_StartPosition;

        void Start()
        {
            m_StartPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if (transform.position.y < -5)
            {
                transform.position = new Vector3(3F, 5.5F, -10.5F);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
        }
    }
}