using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrafoManager : MonoBehaviour
{
    private GrafoTDA<Transform> grafo; // Referencia al grafo
    private Transform bloqueSeleccionado1; // Primer bloque seleccionado
    private Transform bloqueSeleccionado2; // Segundo bloque seleccionado

    void Start()
    {
        grafo = new Grafo();
        grafo.InicializarGrafo();

        // Registrar los bloques que ya están en la escena
        foreach (var bloque in FindObjectsOfType<BloqueInteractivo>())
        {
            grafo.AgregarVertice(bloque.transform);
        }
    }

    void Update()
    {
        // Detectar clic del jugador
        if (Input.GetMouseButtonDown(0))
        {
            DetectarBloque();
        }

        // Si se seleccionaron dos bloques, conectarlos
        if (bloqueSeleccionado1 != null && bloqueSeleccionado2 != null)
        {
            CrearConexion();
            ResetSeleccion();
        }
    }

    // Detectar qué bloque seleccionó el jugador
    private void DetectarBloque()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Verifica si el objeto tiene el componente BloqueInteractivo
            var bloqueInteractivo = hit.transform.GetComponent<BloqueInteractivo>();

            if (bloqueInteractivo != null)
            {
                var bloque = hit.transform;

                if (bloqueSeleccionado1 == null)
                {
                    bloqueSeleccionado1 = bloque;
                }
                else if (bloqueSeleccionado2 == null && bloque != bloqueSeleccionado1)
                {
                    bloqueSeleccionado2 = bloque;
                }
            }
        }
    }

    // Crear una conexión entre los bloques seleccionados
    private void CrearConexion()
    {
        int peso = CalcularPeso(bloqueSeleccionado1.position, bloqueSeleccionado2.position);

        grafo.AgregarArista(bloqueSeleccionado1, bloqueSeleccionado2, peso);
        Debug.Log($"Conexión creada entre {bloqueSeleccionado1.name} y {bloqueSeleccionado2.name} con peso {peso}");
    }

    // Calcular el peso de la conexión
    private int CalcularPeso(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.RoundToInt(Vector3.Distance(pos1, pos2)); // Peso basado en la distancia
    }

    // Restablecer la selección de bloques
    private void ResetSeleccion()
    {
        bloqueSeleccionado1 = null;
        bloqueSeleccionado2 = null;
    }
}
