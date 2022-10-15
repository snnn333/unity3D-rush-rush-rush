using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float rotateSpeed = 2f;
    public int moveSpeed = 10;
    public float moveDistance = 0.5f;
    private int clock = 0;
    public Vector3 startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
        var position = transform.position;
        position.y = startPosition.y + Mathf.Cos(clock/180f * Mathf.PI)*moveDistance;
        transform.position = position;
        clock = (clock + moveSpeed) % 360 ;
    }
}
