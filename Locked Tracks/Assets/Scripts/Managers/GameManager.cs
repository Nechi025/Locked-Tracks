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
        // Inicializar tiempo de inicio
        tiempoInicio = Time.time;

        // Ocultar el panel de resultados al inicio
        resultsPanel.SetActive(false);

        Debug.Log("GameManager iniciado correctamente.");
    }

    public int CalcularTiempoFinal()
    {
        // Calcular tiempo transcurrido desde el inicio
        float tiempoFinal = Time.time - tiempoInicio;
        return Mathf.RoundToInt(tiempoFinal);
    }

    public void TerminarPartida()
    {
        // Calcular tiempo final y formatearlo
        int tiempoFinal = CalcularTiempoFinal();
        string tiempoFormateado = FormatearTiempo(tiempoFinal);

        // Registrar el puntaje en el sistema
        scoreManager.RegistrarPuntaje(tiempoFinal);

        // Obtener el highscore actualizado
        int highscore = scoreManager.CargarHighscore();

        // Obtener el top 3 formateado
        List<string> topTresFormateado = scoreManager.ObtenerTopHighScoreFormateado();

        // Mostrar resultados en el panel
        resultsPanel.SetActive(true);
        resultsManager.MostrarResultados(tiempoFinal, highscore, topTresFormateado);

        // Pausar el juego
        Time.timeScale = 0;

        Debug.Log("Partida terminada. Resultados mostrados.");
    }

    private string FormatearTiempo(int tiempoEnSegundos)
    {
        int minutos = tiempoEnSegundos / 60;
        int segundos = tiempoEnSegundos % 60;
        return string.Format("{0:D2}:{1:D2}", minutos, segundos);
    }

    public void ReiniciarJuego()
    {
        // Restablecer el tiempo del juego
        Time.timeScale = 1;
        Debug.Log("Juego reiniciado.");
    }
}