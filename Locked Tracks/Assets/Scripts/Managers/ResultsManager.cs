using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    public TMP_Text highscoreText; 
    public TMP_Text currentScoreText; 

    public void MostrarResultados(int puntajeFinal, int highscore)
    {
        currentScoreText.text = "Puntaje Final: " + FormatearTiempo(puntajeFinal);
        highscoreText.text = "Highscore: " + FormatearTiempo(highscore);
    }

    private string FormatearTiempo(int tiempoEnSegundos)
    {
        int minutos = tiempoEnSegundos / 60;
        int segundos = tiempoEnSegundos % 60;
        return string.Format("{0:D2}:{1:D2}", minutos, segundos);
    }
}