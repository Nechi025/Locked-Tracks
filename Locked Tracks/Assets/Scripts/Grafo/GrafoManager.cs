using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrafoManager : MonoBehaviour
{
    private GrafoTDA<Transform> grafo; // Referencia al grafo
    private Transform bloqueSeleccionado1; // Inicio
    private Transform bloqueSeleccionado2; // Final
    public float distanciaConexion = 0.5f;
    public LayerMask capaBloques;

    private Dijkstra<Transform> dijkstra;

    public Material materialNormal; // Material para el estado normal
    public Material materialConectado; // Material para el estado conectado

    public CameraLook cl;

    public List<Transform> claveDelPuzzle; // Lista con la secuencia correcta
    private List<Transform> caminoJugador = new List<Transform>();

    private bool conectando = false;
    private bool desconectando = false;

    void Start()
    {
        grafo = new Grafo();
        grafo.InicializarGrafo();

        // Registrar los bloques que ya están en la escena
        foreach (var bloque in FindObjectsOfType<BloqueInteractivo>())
        {
            grafo.AgregarVertice(bloque.transform);
        }
        ConectarVertices();

        dijkstra = new Dijkstra<Transform>();
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
            foreach (var vertice in grafo.ObtenerVertices())
            {
                Debug.Log($"Bloque: {vertice}");
                foreach (var (vecino, peso) in grafo.ObtenerAdyacentes(vertice))
                {
                    Debug.Log($" -> Conectado a: {vecino} con peso: {peso}");
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)) // Botón derecho para desconectar
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

        if (Input.GetKeyDown(KeyCode.Space)) // Presionar espacio para calcular la ruta
        {
            if (bloqueSeleccionado1 != null && bloqueSeleccionado2 != null)
            {
                List<Transform> ruta = dijkstra.CalcularRuta(grafo, bloqueSeleccionado1, bloqueSeleccionado2);

                if (ruta != null)
                {
                    Debug.Log("Ruta encontrada:");
                    foreach (var bloque in ruta)
                    {
                        Debug.Log(bloque.name);
                        ResaltarBloque(bloque);
                    }
                    ResetSeleccion();
                }
                else
                {
                    Debug.LogWarning("No se encontró una ruta.");
                }
            }
        }

        // Si se seleccionaron dos bloques, conectarlos
        /*if (bloqueSeleccionado1 != null && bloqueSeleccionado2 != null && conectando == true)
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
        }*/
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
                    ResaltarBloque(bloqueSeleccionado1);
                }
                else if (bloqueSeleccionado2 == null && bloque != bloqueSeleccionado1)
                {
                    bloqueSeleccionado2 = bloque;
                    ResaltarBloque(bloqueSeleccionado2);
                }
            }
        }
    }

    private void ConectarVertices()
    {
        Collider[] bloques = Physics.OverlapSphere(transform.position, Mathf.Infinity, capaBloques);

        // Conectar bloques adyacentes
        foreach (var bloque in bloques)
        {
            foreach (var vecino in bloques)
            {
                if (bloque != vecino)
                {
                    int distancia = CalcularPeso(bloque.transform.position, vecino.transform.position);
                    if (distancia <= distanciaConexion)
                    {
                        grafo.AgregarArista(bloque.transform, vecino.transform, distancia);
                        Debug.Log("Se agrego arista");
                    }
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
        RegistrarConexion(bloqueSeleccionado1, bloqueSeleccionado2);
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

        Debug.Log("¡Puzzle resuelto!");
        cl.canMove = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.instance.TerminarPartida();
    }

    public void RegistrarConexion(Transform bloque1, Transform bloque2)
    {
        if (!caminoJugador.Contains(bloque1))
        {
            caminoJugador.Add(bloque1);
            ResaltarBloque(bloque1);
        }
            

        if (!caminoJugador.Contains(bloque2))
        {
            caminoJugador.Add(bloque2);
            ResaltarBloque(bloque2);
        }
            

        // Opcional: Dibujar línea o feedback visual
        Debug.DrawLine(bloque1.position, bloque2.position, Color.green, 2f);
    }

    public void DesconectarVertices(Transform bloque1, Transform bloque2)
    {
        // Verifica si ambos bloques están en el camino
        if (caminoJugador.Contains(bloque1) && caminoJugador.Contains(bloque2))
        {
            // Verificar si hay una conexión lógica en el grafo
            if (grafo.ExisteArista(bloque1, bloque2))
            {
                grafo.EliminarArista(bloque1, bloque2); // Elimina la arista en el grafo
                Debug.Log($"Conexión entre {bloque1.name} y {bloque2.name} eliminada.");

                // Elimina los bloques del camino del jugador si no están conectados a otros
                if (!TieneOtrasConexiones(bloque1))
                {
                    caminoJugador.Remove(bloque1);
                    RestablecerMaterial(bloque1);
                }
                    

                if (!TieneOtrasConexiones(bloque2))
                {
                    caminoJugador.Remove(bloque2);
                    RestablecerMaterial(bloque2);
                }
                    

                // Feedback visual opcional
                Debug.DrawLine(bloque1.position, bloque2.position, Color.red, 2f);
            }
            else
            {
                Debug.Log("No hay conexión para eliminar.");
            }
        }
    }

    // Verifica si un bloque tiene otras conexiones activas
    private bool TieneOtrasConexiones(Transform bloque)
    {
        return grafo.ObtenerAdyacentes(bloque).Count > 0;
    }

    private void ResaltarBloque(Transform bloque)
    {
        var renderer = bloque.GetComponent<Renderer>();
        if (renderer != null)
        {
            materialNormal = renderer.material;
            renderer.material = materialConectado;
        }
    }
    private void RestablecerMaterial(Transform bloque)
    {
        var renderer = bloque.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = materialNormal;
        }
    }
}
