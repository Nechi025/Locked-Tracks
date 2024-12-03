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
    public Material materialSeleccionado; // Material para el estado normal
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
            ValidarSolucion();
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
                        ResaltarBloque(bloque, materialConectado);
                        if (!caminoJugador.Contains(bloque))
                        {
                            caminoJugador.Add(bloque);
                        }
                    }
                    ResetSeleccion();

                    var tren = FindObjectOfType<Tren>();
                    if (tren != null)
                    {
                        tren.IniciarMovimiento(ruta);
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontró una ruta.");
                }
            }
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
                    ResaltarBloque(bloqueSeleccionado1, materialSeleccionado);
                }
                else if (bloqueSeleccionado2 == null && bloque != bloqueSeleccionado1)
                {
                    bloqueSeleccionado2 = bloque;
                    ResaltarBloque(bloqueSeleccionado2, materialSeleccionado);
                }
            }

            var bloqueReset = hit.transform.GetComponent<BloqueReset>();

            if (bloqueReset != null)
            {
                ResetearCamino();
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
                        if (distancia < 1)
                        {
                            distancia = 1;
                        }

                        grafo.AgregarArista(bloque.transform, vecino.transform, distancia);
                        Debug.Log("Se agrego arista");
                    }
                }
            }
        }
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
    public void ValidarSolucion()
    {
        // Asegúrate de que la cantidad de bloques coincida
        if (caminoJugador.Count != claveDelPuzzle.Count)
        {
            Debug.Log("No resuelto: cantidad de bloques incorrecta.");
            return;
        }

        // Compara bloque por bloque en el orden
        for (int i = 0; i < claveDelPuzzle.Count; i++)
        {
            if (caminoJugador[i] != claveDelPuzzle[i].transform)
            {
                Debug.Log($"No resuelto: diferencia en el bloque {i + 1}. Esperado: {claveDelPuzzle[i].name}, Jugador: {caminoJugador[i].name}");
                return;
            }
        }

        // Si pasa todas las comprobaciones, la solución es correcta
        Debug.Log("¡Puzzle resuelto!");
        cl.canMove = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.instance.TerminarPartida();
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

    private void ResaltarBloque(Transform bloque, Material material)
    {
        var renderer = bloque.GetComponent<Renderer>();
        if (renderer != null)
        {
            //materialNormal = renderer.material;
            renderer.material = material;
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

    public void ResetearCamino()
    {
        // Restablecer materiales a su estado original
        foreach (var bloque in caminoJugador)
        {
            RestablecerMaterial(bloque);
        }

        // Limpiar la lista de bloques del camino del jugador
        caminoJugador.Clear();

        // También puedes resetear el cubo (tren) a su posición inicial si deseas
        var tren = FindObjectOfType<Tren>();
        if (tren != null)
        {
            tren.IniciarMovimiento(new List<Transform>()); // Pasar una lista vacía para detener el tren
        }
        tren.transform.position = tren.posicionInicial;

        // Opcionalmente, si deseas resetear las selecciones de los bloques
        ResetSeleccion();

        Debug.Log("Camino reiniciado.");
    }
}
