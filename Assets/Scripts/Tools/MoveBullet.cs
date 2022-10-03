using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    public float m_Speed = 0.1f;
    public Vector3 m_Direction = Vector3.up;
    public float sleepSeconds = 0.2f;

    Vector3 m_StartPosition;
    bool IsMoving = false;
   

    void Start()
    {
        m_StartPosition = transform.position;
        // Sleep for some time before moving the cannon
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        //Wait for 4 seconds
        yield return new WaitForSecondsRealtime(sleepSeconds);
        IsMoving = true;
    }

    void FixedUpdate()
    {   
        if (IsMoving == true) {
            // Move the cannong according to the direction
            transform.position += m_Direction * m_Speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cannon Receiver"))
        {
            transform.position = m_StartPosition;
            IsMoving = false;
            sleepSeconds = 0.5f;
            StartCoroutine(waiter());
        }
        else{
            transform.position = m_StartPosition;
            IsMoving = false;
            sleepSeconds = 0.5f;
            StartCoroutine(waiter());
        }
    }
}
