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

        // Cargar puntajes persistidos
        CargarTop3();
        int highscore = CargarHighscore();

        // Agregar el highscore al árbol si no es el valor por defecto
        if (highscore != int.MaxValue)
        {
            arbolPuntajes.AgregarElem(highscore, FormatearTiempo(highscore));
        }

        Debug.Log("Árbol inicializado con los puntajes cargados.");
    }

    public void RegistrarPuntaje(int puntaje)
    {
        // Agregar nuevo puntaje al árbol
        arbolPuntajes.AgregarElem(puntaje, FormatearTiempo(puntaje));

        // Actualizar y guardar el highscore
        GuardarHighscore(puntaje);

        // Guardar el top 3 actualizado
        GuardarTop3();
    }

    public void GuardarTop3()
    {
        List<int> topTres = arbolPuntajes.TopHighScore();

        for (int i = 0; i < 3; i++)
        {
            if (i < topTres.Count)
            {
                PlayerPrefs.SetInt("Top3_" + i, topTres[i]);
            }
            else
            {
                PlayerPrefs.DeleteKey("Top3_" + i); // Elimina claves innecesarias
            }
        }
        PlayerPrefs.Save();
        Debug.Log("Top 3 actualizado: " + string.Join(", ", topTres));
    }

    public void CargarTop3()
    {
        for (int i = 0; i < 3; i++)
        {
            int puntaje = PlayerPrefs.GetInt("Top3_" + i, -1);
            if (puntaje != -1 && !arbolPuntajes.Existe(puntaje)) // Verifica existencia
            {
                arbolPuntajes.AgregarElem(puntaje, FormatearTiempo(puntaje));
            }
        }
        Debug.Log("Top 3 cargado en el árbol.");
    }

    public void GuardarHighscore(int puntaje)
    {
        // Cargar el highscore actual desde PlayerPrefs
        int highscoreActual = PlayerPrefs.GetInt("Highscore", int.MaxValue);

        // Verificar si el nuevo puntaje es mejor
        if (puntaje < highscoreActual)
        {
            PlayerPrefs.SetInt("Highscore", puntaje);
            PlayerPrefs.Save();
            Debug.Log("Nuevo highscore guardado: " + puntaje);
        }
    }

    public int CargarHighscore()
    {
        int highscore = PlayerPrefs.GetInt("Highscore", int.MaxValue);
        Debug.Log("Highscore cargado: " + highscore);
        return highscore;
    }

    public List<string> ObtenerTopHighScoreFormateado()
    {
        // Obtener el top 3 del árbol y formatearlo
        List<int> topTres = arbolPuntajes.TopHighScore();
        List<string> topFormateado = new List<string>();

        foreach (int puntaje in topTres)
        {
            if (puntaje != -1) // Ignorar valores vacíos
            {
                topFormateado.Add(FormatearTiempo(puntaje));
            }
        }

        return topFormateado;
    }

    private string FormatearTiempo(int tiempoEnSegundos)
    {
        int minutos = tiempoEnSegundos / 60;
        int segundos = tiempoEnSegundos % 60;
        return string.Format("{0:D2}:{1:D2}", minutos, segundos);
    }
}