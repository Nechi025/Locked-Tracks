using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrafoManager : MonoBehaviour
{
    private GrafoTDA<Transform> grafo; // Referencia al grafo
    private Transform bloqueSeleccionado1; // Primer bloque seleccionado
    private Transform bloqueSeleccionado2; // Segundo bloque seleccionado

    public CameraLook cl;

    public List<Transform> claveDelPuzzle; // Lista con la secuencia correcta
    private List<Transform> caminoJugador = new List<Transform>();

    private bool conectando = false;
    private bool desconectando = false;

    void Start()
    {
        grafo = new Grafo();
        grafo.InicializarGrafo();

        // Registrar los bloques que ya est�n en la escena
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
            if (!desconectando)
            {
                DetectarBloque();
                conectando = true;
            }
        }
        else if (Input.GetMouseButtonDown(1)) // Bot�n derecho para desconectar
        {
            if (!conectando)
            {
                DetectarBloque();
                desconectando = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return)) // Presiona "Enter" para validar
        {
            ValidarSolucion(caminoJugador);
        }

        // Si se seleccionaron dos bloques, conectarlos
        if (bloqueSeleccionado1 != null && bloqueSeleccionado2 != null && conectando == true)
        {
            CrearConexion();
            ResetSeleccion();
            conectando = false;
        }

        if (bloqueSeleccionado1 != null && bloqueSeleccionado2 != null && desconectando == true)
        {
            DesconectarVertices(bloqueSeleccionado1, bloqueSeleccionado2);
            ResetSeleccion();
            desconectando = false;
        }
    }

    // Detectar qu� bloque seleccion� el jugador
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

    // Crear una conexi�n entre los bloques seleccionados
    private void CrearConexion()
    {
        int peso = CalcularPeso(bloqueSeleccionado1.position, bloqueSeleccionado2.position);

        grafo.AgregarArista(bloqueSeleccionado1, bloqueSeleccionado2, peso);
        Debug.Log($"Conexi�n creada entre {bloqueSeleccionado1.name} y {bloqueSeleccionado2.name} con peso {peso}");
        RegistrarConexion(bloqueSeleccionado1, bloqueSeleccionado2);
    }

    // Calcular el peso de la conexi�n
    private int CalcularPeso(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.RoundToInt(Vector3.Distance(pos1, pos2)); // Peso basado en la distancia
    }

    // Restablecer la selecci�n de bloques
    private void ResetSeleccion()
    {
        bloqueSeleccionado1 = null;
        bloqueSeleccionado2 = null;
    }
    public void ValidarSolucion(List<Transform> caminoJugador)
    {
        if (caminoJugador.Count != claveDelPuzzle.Count)
        {
            Debug.Log("La secuencia es incorrecta.");
            return;
        }

        for (int i = 0; i < claveDelPuzzle.Count; i++)
        {
            if (caminoJugador[i] != claveDelPuzzle[i])
            {
                Debug.Log("La secuencia es incorrecta.");
                return;
            }
        }

        Debug.Log("�Puzzle resuelto!");
        cl.canMove = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.instance.TerminarPartida();
    }

    public void RegistrarConexion(Transform bloque1, Transform bloque2)
    {
        if (!caminoJugador.Contains(bloque1))
            caminoJugador.Add(bloque1);

        if (!caminoJugador.Contains(bloque2))
            caminoJugador.Add(bloque2);

        // Opcional: Dibujar l�nea o feedback visual
        Debug.DrawLine(bloque1.position, bloque2.position, Color.green, 2f);
    }

    public void DesconectarVertices(Transform bloque1, Transform bloque2)
    {
        // Verifica si ambos bloques est�n en el camino
        if (caminoJugador.Contains(bloque1) && caminoJugador.Contains(bloque2))
        {
            // Verificar si hay una conexi�n l�gica en el grafo
            if (grafo.ExisteArista(bloque1, bloque2))
            {
                grafo.EliminarArista(bloque1, bloque2); // Elimina la arista en el grafo
                Debug.Log($"Conexi�n entre {bloque1.name} y {bloque2.name} eliminada.");

                // Elimina los bloques del camino del jugador si no est�n conectados a otros
                if (!TieneOtrasConexiones(bloque1))
                    caminoJugador.Remove(bloque1);

                if (!TieneOtrasConexiones(bloque2))
                    caminoJugador.Remove(bloque2);

                // Feedback visual opcional
                Debug.DrawLine(bloque1.position, bloque2.position, Color.red, 2f);
            }
            else
            {
                Debug.Log("No hay conexi�n para eliminar.");
            }
        }
    }

    // Verifica si un bloque tiene otras conexiones activas
    private bool TieneOtrasConexiones(Transform bloque)
    {
        return grafo.ObtenerAdyacentes(bloque).Count > 0;
    }
}
