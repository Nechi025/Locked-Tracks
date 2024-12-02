using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private ABB arbolPuntajes;

    void Start()
    {
        // Inicializar el árbol de puntajes
        arbolPuntajes = new ABB();
        arbolPuntajes.InicializarArbol();

        // Cargar el highscore al iniciar
        int highscore = CargarHighscore();
        if (highscore != int.MaxValue) // Si hay un highscore registrado
        {
            arbolPuntajes.AgregarElem(highscore, FormatearTiempo(highscore));
        }
    }

    public void RegistrarPuntaje(int puntaje, string tiempo)
    {
        // Agregar el puntaje al árbol
        arbolPuntajes.AgregarElem(puntaje, tiempo);

        // Guardar el highscore si es necesario
        GuardarHighscore(puntaje);
    }

    public int ObtenerHighscore()
    {
        if (arbolPuntajes.ArbolVacio())
        {
            return int.MaxValue; // No hay puntajes registrados
        }
        return arbolPuntajes.Menor(arbolPuntajes.raiz);
    }

    public void GuardarHighscore(int puntaje)
    {
        int highscoreGuardado = PlayerPrefs.GetInt("Highscore", int.MaxValue);
        if (puntaje < highscoreGuardado) // Solo guarda si el puntaje es menor (mejor tiempo)
        {
            PlayerPrefs.SetInt("Highscore", puntaje);
            PlayerPrefs.Save();
        }
    }


    public int CargarHighscore()
    {
        return PlayerPrefs.GetInt("Highscore", int.MaxValue);
    }

    public List<string> ObtenerTopHighScoreFormateado()
    {
        List<int> topTres = arbolPuntajes.TopHighScore();
        List<string> topHighScoreFormateado = new List<string>();

        foreach (int puntaje in topTres)
        {
            topHighScoreFormateado.Add(FormatearTiempo(puntaje));
        }

        return topHighScoreFormateado;
    }

    private string FormatearTiempo(int tiempoEnSegundos)
    {
        int minutos = tiempoEnSegundos / 60;
        int segundos = tiempoEnSegundos % 60;
        return string.Format("{0:D2}:{1:D2}", minutos, segundos);
    }
}