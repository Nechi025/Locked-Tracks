using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPuzzle : MonoBehaviour, IInteractable
{
    public GameObject canvas;
    public CameraLook cl;
    public bool isInteracting = false;


    public void Interact()
    {
        if (isInteracting == false)
        {
            canvas.SetActive(true);
            cl.canMove = false;
            Cursor.lockState = CursorLockMode.None;
            isInteracting = true;
        }
        /*else
        {
            canvas.SetActive(false);
            cl.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            isInteracting = false;
        }*/
    }
}
