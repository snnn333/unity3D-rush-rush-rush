using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Makes a transform oscillate relative to its start position
public class MovePlatform : MonoBehaviour
{
    public float m_Amplitude = 1.0f;
    public float m_Period = 1.0f;
    public float m_WaitTime = 1.0f;
    public Vector3 m_Direction = Vector3.up;
    Vector3 m_StartPosition;
    private float m_StartTime;
    private bool m_IsMoving;

    void Start()
    {
        m_StartPosition = transform.position;
        m_StartTime = Time.time;
    }

    void FixedUpdate()
    {
        float interval = Time.time - m_StartTime;
        if (interval >= m_Period) {
            StartCoroutine(Wait());
        }
        else {
            transform.position += m_Direction * m_Amplitude / m_Period;
        }
    }

    private IEnumerator Wait()
        {
            m_IsMoving = false;
            yield return new WaitForSeconds(m_WaitTime);
            m_IsMoving = true;
            m_StartTime = Time.time;
            m_Direction = -1 * m_Direction;
        }
}
