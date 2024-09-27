using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] float mouseSensitive = 80f;

    public Transform playerBody;

    float xRotation = 0;

    public bool canMove = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            CameraMove();
        }
        

    }

    private void CameraMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitive * Time.deltaTime;

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitive * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
