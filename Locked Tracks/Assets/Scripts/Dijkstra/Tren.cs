using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tren : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de movimiento
    private List<Transform> camino; // Lista del camino a seguir
    private int indiceActual = 0; // Índice del nodo actual

    private bool moviendo = false; // Para saber si está en movimiento

    public Vector3 posicionInicial;

    // Inicializar el tren con un camino
    public void IniciarMovimiento(List<Transform> nuevoCamino)
    {
        if (nuevoCamino == null || nuevoCamino.Count == 0)
        {
            Debug.LogWarning("El camino está vacío. No se puede iniciar el movimiento.");
            return;
        }

        camino = nuevoCamino;
        indiceActual = 0;
        transform.position = new Vector3(camino[0].position.x + 0.15f, camino[0].position.y, camino[0].position.z); // Colocar el tren en el primer nodo
        moviendo = true;
    }

    private void Start()
    {
        posicionInicial = transform.position;
    }
    void Update()
    {
        if (moviendo && camino != null && indiceActual < camino.Count - 1)
        {
            MoverHaciaNodo(camino[indiceActual + 1]);
        }
    }

    private void MoverHaciaNodo(Transform nodoDestino)
    {
        // Mueve el tren hacia el nodo destino
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(nodoDestino.position.x + 0.15f, nodoDestino.position.y, nodoDestino.position.z), velocidad * Time.deltaTime);

        // Si llega al nodo destino, avanzar al siguiente
        if (Vector3.Distance(transform.position, nodoDestino.position) < 0.2f)
        {
            indiceActual++;
            if (indiceActual >= camino.Count - 1)
            {
                moviendo = false; // Detener el movimiento al llegar al último nodo
                Debug.Log("Tren llegó al destino.");
            }
        }
    }
}
