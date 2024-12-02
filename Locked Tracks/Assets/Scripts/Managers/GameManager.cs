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

        resultsPanel.SetActive(false);
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TerminarPartida();
            Debug.Log("Terminado");
        }*/
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

        // Registrar el tiempo como puntaje
        scoreManager.RegistrarPuntaje(tiempoFinal, tiempoFormateado);

        // Obtener el highscore actualizado
        int highscore = scoreManager.ObtenerHighscore();

        // Obtener los tres mejores puntajes
        List<string> topTres = scoreManager.ObtenerTopHighScoreFormateado();

        // Mostrar resultados en el panel
        resultsPanel.SetActive(true);
        resultsManager.MostrarResultados(tiempoFinal, highscore, topTres);

        // Detener el tiempo de juego
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
        Time.timeScale = 1; // Restaura el tiempo del juego
    }
}