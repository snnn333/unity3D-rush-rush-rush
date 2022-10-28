using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    public float m_Speed = 0.1f;
    public Vector3 m_Direction = Vector3.up;
    public float sleepSeconds = 1f;

    public float maxDistance = 10.0f;

    public Transform StartPosition = null;

    [Tooltip("Effect to start teleport.")] public GameObject TeleportEffect;

    private Vector3 m_StartPosition;
    private bool IsMoving = false;
   

    void Start()
    {
        m_StartPosition = transform.position;
        // Sleep for some time before moving the cannon
        StartCoroutine(WaitThenMove());
    }

    IEnumerator WaitThenMove()
    {
        // Wait some time then move the bullet
        IsMoving = false;
        yield return new WaitForSecondsRealtime(sleepSeconds);
        IsMoving = true;
    }

    void FixedUpdate()
    {   
        if (IsMoving == true) {
            // Move the cannong according to the direction
            transform.position += m_Direction * m_Speed;
        }

        // Stop moving the bullet after out of range
        if (Vector3.Distance(m_StartPosition, transform.position) >= maxDistance) {
            Reset();
        }
    }

    private void Reset() {
        // Display the smoke effect when the bullet explodes
        if (TeleportEffect)
        {
            Instantiate(TeleportEffect, transform.position, transform.rotation);
        }

        // Reset to the original position
        if (StartPosition == null) {
            transform.position = m_StartPosition;
        } else {
            transform.position = StartPosition.position;
        }
        // Stop moving the bullet
        StartCoroutine(WaitThenMove());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cannon Receiver"))
        {
            Reset();
        }
    }
}
