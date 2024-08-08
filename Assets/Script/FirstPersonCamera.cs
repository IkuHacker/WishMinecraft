using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    // Variables
    public Transform player;

    private Vector2 cameraTurn;
    public float maxRotationAngleX;
    public float mouseSensitivity;

    public Transform cameraTransform;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Collect Mouse Input

        cameraTurn.y += Input.GetAxisRaw("Mouse X") * mouseSensitivity;

        cameraTurn.x += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        cameraTurn.x = Mathf.Clamp(cameraTurn.x, -maxRotationAngleX, maxRotationAngleX);


        cameraTransform.localRotation = Quaternion.Euler(cameraTurn.x, -cameraTurn.y, 0f);

        player.localRotation = Quaternion.Euler(0f, -cameraTurn.y, 0f);


    }
}