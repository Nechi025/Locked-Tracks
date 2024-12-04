using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    public TMP_Text highscoreText; 
    public TMP_Text currentScoreText;
    public TMP_Text topScoresText;


    public void MostrarResultados(int puntajeFinal, int highscore, List<string> topTres)
    {
        currentScoreText.text = "Puntaje Final: " + FormatearTiempo(puntajeFinal);
        highscoreText.text = "Highscore: " + FormatearTiempo(highscore);

        string topScores = "Top 3:\n";
        for (int i = 0; i < topTres.Count; i++)
        {
            topScores += (i + 1) + ". " + topTres[i] + "\n";
        }
        topScoresText.text = topScores;
    }


    private string FormatearTiempo(int tiempoEnSegundos)
    {
        int minutos = tiempoEnSegundos / 60;
        int segundos = tiempoEnSegundos % 60;
        return string.Format("{0:D2}:{1:D2}", minutos, segundos);
    }

}