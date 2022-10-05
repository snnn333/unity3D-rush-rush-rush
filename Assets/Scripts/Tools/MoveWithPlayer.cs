using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveWithPlayer : MonoBehaviour
{
    public float m_Speed = 0.1f;
    public Vector3 m_Direction = Vector3.up;
    public Vector3 m_PositionShift;

    Vector3 m_StartPosition;



    GameObject player;

    void Start()
    {

        player = GameObject.Find("Player");
        m_StartPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (Math.Abs(player.transform.position.y - m_StartPosition.y) < 10.0F)
        {
            // Move the cannong according to the direction
            transform.position= new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
        }
    }
}
