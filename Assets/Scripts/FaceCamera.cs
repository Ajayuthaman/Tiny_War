using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        // Get the main camera in the scene
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // Face the health bar towards the camera
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }
}

