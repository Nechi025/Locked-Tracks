using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra<T>
{
    public List<T> CalcularRuta(GrafoTDA<T> grafo, T inicio, T destino)
    {
        // Diccionarios para distancias y predecesores
        Dictionary<T, int> distancias = new Dictionary<T, int>();
        Dictionary<T, T> predecesores = new Dictionary<T, T>();

        // Conjunto de vértices no visitados
        HashSet<T> noVisitados = new HashSet<T>();

        // Inicializar distancias
        foreach (var vertice in grafo.ObtenerVertices())
        {
            distancias[vertice] = int.MaxValue; // Distancia infinita
            predecesores[vertice] = default; // Sin predecesores al inicio
            noVisitados.Add(vertice); // Todos los vértices están no visitados
        }

        // La distancia al nodo inicial es 0
        distancias[inicio] = 0;

        // Mientras existan vértices no visitados
        while (noVisitados.Count > 0)
        {
            // Encontrar el vértice con la menor distancia
            T verticeActual = default;
            int menorDistancia = int.MaxValue;

            foreach (var vertice in noVisitados)
            {
                if (distancias[vertice] < menorDistancia)
                {
                    menorDistancia = distancias[vertice];
                    verticeActual = vertice;
                }
            }

            // Si no hay vértice válido o llegamos al destino, terminar
            if (verticeActual == null || verticeActual.Equals(destino))
            {
                break;
            }

            // Marcar el vértice actual como visitado
            noVisitados.Remove(verticeActual);

            // Actualizar distancias a los vecinos
            foreach (var (vecino, peso) in grafo.ObtenerAdyacentes(verticeActual))
            {
                if (noVisitados.Contains(vecino))
                {
                    int distanciaTentativa = distancias[verticeActual] + peso;
                    if (distanciaTentativa < distancias[vecino])
                    {
                        distancias[vecino] = distanciaTentativa;
                        predecesores[vecino] = verticeActual;
                    }
                }
            }
        }

        // Reconstruir el camino desde el destino hacia el inicio
        List<T> camino = new List<T>();
        T pasoActual = destino;

        while (!EqualityComparer<T>.Default.Equals(pasoActual, default))
        {
            camino.Insert(0, pasoActual); // Insertar al inicio del camino
            pasoActual = predecesores.ContainsKey(pasoActual) ? predecesores[pasoActual] : default;
        }

        // Si el inicio no está en el camino, no hay ruta
        if (camino.Count == 0 || !camino[0].Equals(inicio))
        {
            Debug.LogWarning("No se encontró un camino entre los puntos dados.");
            return null;
        }

        return camino;
    }
}
