using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransportPlatform : MonoBehaviour
{
    public float m_Speed = 0.1f;
    public Vector3 m_Direction = Vector3.up;
    public Vector3 m_PositionShift;

    Vector3 m_StartPosition;
    bool IsMoving = false;
   

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

        if (transform.position + m_StartPosition == m_PositionShift) {
            IsMoving = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(waiter());
        }
    }

    IEnumerator waiter()
    {
        //Wait for 1 seconds
        yield return new WaitForSecondsRealtime(1);
        IsMoving = true;
    }
}
