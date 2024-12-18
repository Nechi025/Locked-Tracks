using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public CameraLook cam;

    [SerializeField] float speed;

    void Update()
    {
        if (cam.canMove == true)
        {
            Move();
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GameManager.instance.TerminarPartida();
        //}

    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameOver"))
        {
            GameManager.instance.TerminarPartida();
        }
    }
}
