using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grafo : MonoBehaviour, GrafoTDA<Transform>
{
    private Dictionary<Transform, List<(Transform, int)>> adjacencias; // Diccionario de adyacencias
    private List<Transform> vertices; // Lista de v�rtices

    // Inicializar el grafo
    public void InicializarGrafo()
    {
        adjacencias = new Dictionary<Transform, List<(Transform, int)>>(); // Almacena las aristas
        vertices = new List<Transform>(); // Lista de v�rtices
    }

    // Agregar un v�rtice al grafo (sin duplicados)
    public void AgregarVertice(Transform bloque)
    {
        if (!vertices.Contains(bloque)) // Verifica si el v�rtice ya est� en la lista
        {
            vertices.Add(bloque); // Si no est�, lo agrega
            adjacencias[bloque] = new List<(Transform, int)>(); // Inicializa la lista de adyacencias para el v�rtice
        }
    }

    // Eliminar un v�rtice y sus aristas
    public void EliminarVertice(Transform bloque)
    {
        if (vertices.Contains(bloque))
        {
            vertices.Remove(bloque);
            adjacencias.Remove(bloque); // Eliminar las aristas relacionadas con este v�rtice

            // Eliminar las aristas hacia el v�rtice desde otros v�rtices
            foreach (var vertice in adjacencias)
            {
                vertice.Value.RemoveAll(arista => arista.Item1 == bloque);
            }
        }
    }

    // Agregar una arista con peso entre dos v�rtices
    public void AgregarArista(Transform bloque1, Transform bloque2, int peso)
    {
        if (vertices.Contains(bloque1) && vertices.Contains(bloque2))
        {
            adjacencias[bloque1].Add((bloque2, peso));
            adjacencias[bloque2].Add((bloque1, peso));
        }
    }

    // Eliminar una arista entre dos v�rtices
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

    // Verificar si existe una arista entre dos v�rtices
    public bool ExisteArista(Transform bloque1, Transform bloque2)
    {
        return adjacencias.ContainsKey(bloque1) && adjacencias[bloque1].Exists(arista => arista.Item1 == bloque2);
    }

    // Obtener el peso de una arista entre dos v�rtices
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
