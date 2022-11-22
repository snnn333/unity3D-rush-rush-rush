using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransportPlatform : MonoBehaviour
{
    public float m_Speed = 0.1f;
    public Vector3 m_Direction = Vector3.up;
    public float MoveDistance = 10f;

    public bool IsNeedReset = false;    // Reset to the original point after getting to the destination

    private Vector3 m_StartPosition;
    private bool IsMoving = false;
   

    void Start()
    {
        m_StartPosition = transform.position;
    }

    void FixedUpdate()
    {   
        if (IsMoving == true) {
            // Move the cannong according to the direction
            transform.position += m_Direction * m_Speed;
        }

        // When reaching the terminal
        if (Vector3.Distance(m_StartPosition, transform.position) >= MoveDistance) {
            if (IsNeedReset) {
                // Reset to the orignal point
                transform.position = m_StartPosition;
            } else {
                // Switch the moving direction
                m_Direction = m_Direction * -1;
                m_StartPosition = transform.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsMoving == false)
        {
            IsMoving = true;
        }
    }

    IEnumerator waiter()
    {
        //Wait for 1 seconds
        yield return new WaitForSeconds(1);
        IsMoving = true;
    }

    IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(2);
    }
}
