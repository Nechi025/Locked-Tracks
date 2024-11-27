using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grafo : MonoBehaviour, GrafoTDA<Transform>
{
    private Dictionary<Transform, List<(Transform, int)>> adjacencias; // Diccionario de adyacencias
    private List<Transform> vertices; // Lista de vértices

    // Inicializar el grafo
    public void InicializarGrafo()
    {
        adjacencias = new Dictionary<Transform, List<(Transform, int)>>(); // Almacena las aristas
        vertices = new List<Transform>(); // Lista de vértices
    }

    // Agregar un vértice al grafo (sin duplicados)
    public void AgregarVertice(Transform bloque)
    {
        if (!vertices.Contains(bloque)) // Verifica si el vértice ya está en la lista
        {
            vertices.Add(bloque); // Si no está, lo agrega
            adjacencias[bloque] = new List<(Transform, int)>(); // Inicializa la lista de adyacencias para el vértice
        }
    }

    // Eliminar un vértice y sus aristas
    public void EliminarVertice(Transform bloque)
    {
        if (vertices.Contains(bloque))
        {
            vertices.Remove(bloque);
            adjacencias.Remove(bloque); // Eliminar las aristas relacionadas con este vértice

            // Eliminar las aristas hacia el vértice desde otros vértices
            foreach (var vertice in adjacencias)
            {
                vertice.Value.RemoveAll(arista => arista.Item1 == bloque);
            }
        }
    }

    // Agregar una arista con peso entre dos vértices
    public void AgregarArista(Transform bloque1, Transform bloque2, int peso)
    {
        if (vertices.Contains(bloque1) && vertices.Contains(bloque2))
        {
            adjacencias[bloque1].Add((bloque2, peso));
            adjacencias[bloque2].Add((bloque1, peso));
        }
    }

    // Eliminar una arista entre dos vértices
    public void EliminarArista(Transform bloque1, Transform bloque2)
    {
        if (adjacencias.ContainsKey(bloque1))
        {
            adjacencias[bloque1].RemoveAll(arista => arista.Item1 == bloque2);
        }

        if (adjacencias.ContainsKey(bloque2))
        {
            adjacencias[bloque2].RemoveAll(arista => arista.Item1 == bloque1);
        }
    }

    // Verificar si existe una arista entre dos vértices
    public bool ExisteArista(Transform bloque1, Transform bloque2)
    {
        return adjacencias.ContainsKey(bloque1) && adjacencias[bloque1].Exists(arista => arista.Item1 == bloque2);
    }

    // Obtener el peso de una arista entre dos vértices
    public int PesoArista(Transform bloque1, Transform bloque2)
    {
        if (adjacencias.ContainsKey(bloque1))
        {
            var arista = adjacencias[bloque1].Find(ar => ar.Item1 == bloque2);
            if (!arista.Equals(default))
            {
                return arista.Item2; // Retorna el peso de la arista
            }
        }
        return -1; // Si no existe la arista, retorna -1
    }

    public List<(Transform, int)> ObtenerAdyacentes(Transform vertice)
    {
        if (adjacencias.ContainsKey(vertice))
        {
            return adjacencias[vertice];
        }
        return new List<(Transform, int)>();
    }
}
