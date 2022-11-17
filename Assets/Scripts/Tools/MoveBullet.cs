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
    [Tooltip("Effect when the bullet is flying.")] public GameObject MoveEffect;

    private Vector3 m_StartPosition;
    private bool IsMoving = false;

    private void SpawnEffect(GameObject myVFX, float duration)
    {
        if (myVFX != null) {
            GameObject spawnedVFX = Instantiate(myVFX, transform.position, transform.rotation) as GameObject; 
            Destroy(spawnedVFX, duration);
        }
    }
   

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
        yield return new WaitForSeconds(sleepSeconds);
        IsMoving = true;
        // // Display the teleport effect when the bullet starts to move
        // if (TeleportEffect)
        // {
        //     Instantiate(TeleportEffect, transform.position, transform.rotation);
        //     Destroy(TeleportEffect, 1f);
        // }
        // Show the move effect after a second
        if (MoveEffect)
        {
            MoveEffect.SetActive(true);
        }
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
        // Destroy the move effect
        if (MoveEffect)
        {
            MoveEffect.SetActive(false);
        }

        // Display the smoke effect when the bullet explodes
        if (TeleportEffect)
        {
            Instantiate(TeleportEffect, transform.position, transform.rotation);
            Destroy(TeleportEffect, 1f);
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
