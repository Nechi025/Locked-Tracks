using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text tiempoText; // Texto para mostrar el tiempo durante la partida
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // Actualiza el tiempo transcurrido durante la partida
        int tiempoActual = gameManager.CalcularTiempoFinal();
    }
}
