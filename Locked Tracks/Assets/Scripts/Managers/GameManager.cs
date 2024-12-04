using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameObject resultsPanel; // Panel de resultados
    public ResultsManager resultsManager;

    private float tiempoInicio;

    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        tiempoInicio = Time.time; // Registra el inicio del tiempo de la partida

        Debug.Log("GameManager Start ejecutándose.");

        PlayerPrefs.SetInt("Prueba", 12345);
        PlayerPrefs.Save();

        int prueba = PlayerPrefs.GetInt("Prueba", -1);
        Debug.Log("Valor de prueba guardado y recuperado: " + prueba);

        resultsPanel.SetActive(false);

    }


    public int CalcularTiempoFinal()
    {
        float tiempoFinal = Time.time - tiempoInicio; // Tiempo transcurrido en segundos
        return Mathf.RoundToInt(tiempoFinal); // Devuelve un entero redondeado
    }

    public void TerminarPartida()
    {
        int tiempoFinal = CalcularTiempoFinal();
        string tiempoFormateado = FormatearTiempo(tiempoFinal);

        scoreManager.RegistrarPuntaje(tiempoFinal, tiempoFormateado);

        int highscore = scoreManager.ObtenerHighscore();

        List<int> topTres = scoreManager.arbolPuntajes.TopHighScore(); 

        List<string> topTresFormateado = new List<string>();
        foreach (int puntaje in topTres)
        {
            topTresFormateado.Add(FormatearTiempo(puntaje));
        }

        resultsPanel.SetActive(true);
        resultsManager.MostrarResultados(tiempoFinal, highscore, topTresFormateado);

        Time.timeScale = 0;

      
    }

    private string FormatearTiempo(int tiempoEnSegundos)
    {
        int minutos = tiempoEnSegundos / 60;
        int segundos = tiempoEnSegundos % 60;
        return string.Format("{0:D2}:{1:D2}", minutos, segundos);
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1; 
    }

}