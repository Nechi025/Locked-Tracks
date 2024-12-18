using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] bool bomb = false;
    [SerializeField] float timeBomb = 0;

    private void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.remainingTime = Mathf.Max(GameManager.instance.remainingTime, 0); // Asegura que no sea negativo
        }
    }

    private void Update()
    {
        if (!bomb)
        {
            if (GameManager.instance.remainingTime > 0)
            {
                GameManager.instance.remainingTime -= Time.deltaTime;
            }
            else
            {
                GameManager.instance.remainingTime = 0;
                Cursor.lockState = CursorLockMode.None;
                ChangeScene.instance.LoadSceneByName("Lose");
            }

            int minutes = Mathf.FloorToInt(GameManager.instance.remainingTime / 60);
            int seconds = Mathf.FloorToInt(GameManager.instance.remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        else if (bomb)
        {
            if (timeBomb > 0)
            {
                timeBomb -= Time.deltaTime;
            }
            else
            {
                timeBomb = 0;
            }

            int minutes = Mathf.FloorToInt(timeBomb / 60);
            int seconds = Mathf.FloorToInt(timeBomb % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        
    }
}
