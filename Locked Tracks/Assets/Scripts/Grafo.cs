using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grafo<T> : MonoBehaviour, GrafoTDA<T>
{
    private Dictionary<T, List<(T, int)>> adjacencias; // Diccionario de adyacencias
    private List<T> vertices; // Lista de vértices

    // Inicializar el grafo
    public void InicializarGrafo()
    {
        adjacencias = new Dictionary<T, List<(T, int)>>(); // Almacena las aristas
        vertices = new List<T>(); // Lista de vértices
    }

    // Agregar un vértice al grafo (sin duplicados)
    public void AgregarVertice(T v)
    {
        if (!vertices.Contains(v)) // Verifica si el vértice ya está en la lista
        {
            vertices.Add(v); // Si no está, lo agrega
            adjacencias[v] = new List<(T, int)>(); // Inicializa la lista de adyacencias para el vértice
        }
    }

    // Eliminar un vértice y sus aristas
    public void EliminarVertice(T v)
    {
        if (vertices.Contains(v))
        {
            vertices.Remove(v);
            adjacencias.Remove(v); // Eliminar las aristas relacionadas con este vértice

            // Eliminar las aristas hacia el vértice desde otros vértices
            foreach (var vertice in adjacencias)
            {
                vertice.Value.RemoveAll(arista => arista.Item1.Equals(v));
            }
        }
    }

    // Agregar una arista con peso entre dos vértices
    public void AgregarArista(T id, T v1, T v2, int peso)
    {
        if (vertices.Contains(v1) && vertices.Contains(v2))
        {
            adjacencias[v1].Add((v2, peso));
            adjacencias[v2].Add((v1, peso)); // Si es no dirigido
        }
    }

    // Eliminar una arista entre dos vértices
    public void EliminarArista(T v1, T v2)
    {
        if (adjacencias.ContainsKey(v1))
        {
            adjacencias[v1].RemoveAll(arista => arista.Item1.Equals(v2));
        }

        if (adjacencias.ContainsKey(v2))
        {
            adjacencias[v2].RemoveAll(arista => arista.Item1.Equals(v1));
        }
    }

    // Verificar si existe una arista entre dos vértices
    public bool ExisteArista(T v1, T v2)
    {
        return adjacencias.ContainsKey(v1) && adjacencias[v1].Exists(arista => arista.Item1.Equals(v2));
    }

    // Obtener el peso de una arista entre dos vértices
    public int PesoArista(T v1, T v2)
    {
        if (adjacencias.ContainsKey(v1))
        {
            var arista = adjacencias[v1].Find(ar => ar.Item1.Equals(v2));
            if (!arista.Equals(default))
            {
                return arista.Item2; // Retorna el peso de la arista
            }
        }
        return -1; // Si no existe la arista, retorna -1
    }
}
