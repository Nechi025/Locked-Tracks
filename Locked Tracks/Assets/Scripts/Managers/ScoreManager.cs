using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public ABB arbolPuntajes;

    void Start()
    {
        arbolPuntajes = new ABB();
        arbolPuntajes.InicializarArbol();

        arbolPuntajes.TestArbol();

        int highscore = CargarHighscore();
        Debug.Log($"Highscore cargado al iniciar: {highscore}");

        CargarTop3();

        if (highscore != int.MaxValue)
        {
            arbolPuntajes.AgregarElem(highscore, FormatearTiempo(highscore));
        }

        arbolPuntajes.ImprimirArbol();
    }

    public void RegistrarPuntaje(int puntaje, string tiempo)
    {
        
        arbolPuntajes.AgregarElem(puntaje, tiempo);

        GuardarHighscore(puntaje);

        GuardarTop3();

        arbolPuntajes.ImprimirArbol();
    }

    public void GuardarTop3()
    {
        List<int> topTres = arbolPuntajes.TopHighScore();

        for (int i = 0; i < topTres.Count; i++)
        {
            PlayerPrefs.SetInt("Top3_" + i, topTres[i]);
        }

        PlayerPrefs.Save();
        Debug.Log("Top 3 guardado en PlayerPrefs.");
    }

    public void CargarTop3()
    {
        List<int> topTres = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            int puntaje = PlayerPrefs.GetInt("Top3_" + i, -1);  // -1 si no existe
            if (puntaje != -1)
            {
                topTres.Add(puntaje);
            }
        }

        // Si hay menos de tres puntajes, los completamos con -1
        while (topTres.Count < 3)
        {
            topTres.Add(-1);
        }

        // Mostrar el Top 3 en el juego
        Debug.Log("Top 3 cargado: " + string.Join(", ", topTres));
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
        // Cargar el highscore actual desde PlayerPrefs
        int highscoreGuardado = PlayerPrefs.GetInt("Highscore", int.MaxValue);

        if (puntaje < highscoreGuardado)
        {
            PlayerPrefs.SetInt("Highscore", puntaje);
            PlayerPrefs.Save();
            Debug.Log("Nuevo highscore guardado: " + puntaje);
        }
        else
        {
            Debug.Log("El puntaje no es mejor que el highscore actual, pero se guarda en el árbol.");
        }

        // Siempre agrega el puntaje al árbol binario
        arbolPuntajes.AgregarElem(puntaje, FormatearTiempo(puntaje));
        arbolPuntajes.ImprimirArbol();  // Depuración: Verificar el estado del árbol
    }


    public int CargarHighscore()
    {
        int highscore = PlayerPrefs.GetInt("Highscore", int.MaxValue);
        Debug.Log("Highscore cargado: " + highscore);
        return highscore;
    }

    public List<string> ObtenerTopHighScoreFormateado()
    {
        List<int> topTres = arbolPuntajes.TopHighScore();
        List<string> topHighScoreFormateado = new List<string>();

        foreach (int puntaje in topTres)
        {
            if (puntaje != -1) // Ignora los marcadores de valores vacíos
            {
                topHighScoreFormateado.Add(FormatearTiempo(puntaje));
            }
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