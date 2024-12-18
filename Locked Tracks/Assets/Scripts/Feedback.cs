using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback : MonoBehaviour
{
    public TMPro.TextMeshProUGUI feedbackText;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            feedbackText.gameObject.SetActive(true);
            feedbackText.text = "Press Left Click to connect";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            feedbackText.gameObject.SetActive(false);
            feedbackText.text = "";
        }
    }
}
