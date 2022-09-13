using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform CameraTransform;

    private void Start()
    {
        if (Camera.main != null) CameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Face the Camera
        if (CameraTransform)
        {
            transform.rotation = CameraTransform.transform.rotation;
        }
    }
}