using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform interactorSource;
    public float interactRange;
    public TMPro.TextMeshProUGUI interactText;


    void Update()
    {

        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitinfo, interactRange))
        {

            if (hitinfo.collider.gameObject.CompareTag("Panel"))
            {
                hitinfo.collider.gameObject.TryGetComponent(out PanelPuzzle panelPuzzle);

                if (!panelPuzzle.isInteracting)
                {
                    interactText.gameObject.SetActive(true);
                    interactText.text = "Press E to interact";
                }
                

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hitinfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                    {
                        interactObj.Interact();
                        interactText.gameObject.SetActive(false);
                    }
                }
            }

            
        }
        else interactText.gameObject.SetActive(false);


    }
}
